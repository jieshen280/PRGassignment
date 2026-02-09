//==========================================================
// Student Number : S10275159
// Student Name : Aw Chong Boon
// Partner Name : Yip Jie Shen
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using S10275159_PRG2Assignment;

// Global Lists
List<Restaurant> restaurantList = new List<Restaurant>();
List<Customer> customerList = new List<Customer>();
List<Order> allOrders = new List<Order>();

Stack<Order> refundStack = new Stack<Order>();
Dictionary<int, string> orderRestaurantMap = new Dictionary<int, string>(); // Maps OrderID to RestaurantID
int nextOrderId = 1;

// Feature 1 & 2: Load Data at startup
try 
{
    LoadRestaurantsAndMenu();  // Feature 1
    LoadCustomersAndOrders();  // Feature 2
}
catch (Exception ex) { Console.WriteLine("Error loading data: " + ex.Message); }

// Initial Display
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurantList.Count} restaurants loaded!");
int totalFoodItems = 0;
foreach(var r in restaurantList) totalFoodItems += r.Menu.FoodItems.Count;
Console.WriteLine($"{totalFoodItems} food items loaded!");
Console.WriteLine($"{customerList.Count} customers loaded!");
Console.WriteLine($"{allOrders.Count} orders loaded!");

// Main Loop
while (true)
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Process unspecified orders (Advanced)");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
    
    string option = Console.ReadLine() ?? "";

    if (option == "1") ListRestaurantsAndMenu(); // Feature 3
    else if (option == "2") ListAllOrders(); // Feature 4
    else if (option == "3") CreateNewOrder(); // Feature 5
    else if (option == "4") ProcessOrder(); // Feature 6
    else if (option == "5") ModifyOrder(); // Feature 7
    else if (option == "6") DeleteOrder(); // Feature 8
    else if (option == "7") BulkProcessOrders(); // Advanced Feature (a)
    else if (option == "8") DisplayTotalReceipts(); // Advanced Feature (b)
    else if (option == "9") SendCustomerNotifications(); // Bonus Feature (c)
    else if (option == "0") 
    {
        SaveQueuesAndStack();
        break;
    }
    else Console.WriteLine("Invalid option. Please try again.");
}

Console.WriteLine("Exiting...");


// ---------------- Methods ----------------

// ---------------- Methods ----------------

// Feature 1 Done by Jie Shen
void LoadRestaurantsAndMenu()
{
    try
    {
        // Load Restaurants (ID, Name, Email)
        if (File.Exists("restaurants.csv"))
        {
            string[] lines = File.ReadAllLines("restaurants.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                   restaurantList.Add(new Restaurant(parts[1], parts[2], parts[0])); 
                }
            }
        }

        // Load Food Items
        if (File.Exists("fooditems.csv"))
        {
            string[] lines = File.ReadAllLines("fooditems.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 4)
                {
                    string rid = parts[0];
                    string name = parts[1];
                    string desc = parts[2];
                    double price = 0;
                    double.TryParse(parts[3], out price);
                    
                    Restaurant? r = restaurantList.Find(x => x.RestaurantID == rid);
                    if (r != null)
                    {
                        r.Menu.AddFoodItem(new FoodItem(name, desc, price));
                    }
                }
            }
        }
    } 
    catch(Exception ex) { Console.WriteLine($"Error loading restaurants/menu: {ex.Message}"); }
}

// Feature 2 Done by Chong Boon
void LoadCustomersAndOrders()
{
    try
    {
        // ... (Keep existing implementation)
        // Load Customers
        if (File.Exists("customers.csv"))
        {
            string[] lines = File.ReadAllLines("customers.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(',');
                // Format: Name(0), Email(1)
                if (parts.Length >= 2)
                {
                    customerList.Add(new Customer(parts[0], parts[1]));
                }
            }
        }

        // Load Orders
        if (File.Exists("orders.csv"))
        {
            string[] lines = File.ReadAllLines("orders.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                // CSV Parsing Logic
                List<string> parts = new List<string>();
                bool inQuotes = false;
                string currentPart = "";
                foreach(char c in lines[i])
                {
                    if (c == '"') inQuotes = !inQuotes;
                    else if (c == ',' && !inQuotes) 
                    { 
                        parts.Add(currentPart); 
                        currentPart = ""; 
                    }
                    else currentPart += c;
                }
                parts.Add(currentPart);

                if (parts.Count >= 10) // Fixed to 10 to allow access to parts[9]
                {
                    int oid = 0;
                    int.TryParse(parts[0], out oid);
                    if (oid >= nextOrderId) nextOrderId = oid + 1;

                    string email = parts[1];
                    string rid = parts[2];
                    string dDate = parts[3];
                    string dTime = parts[4];
                    string addr = parts[5];
                    string createdDt = parts[6];
                    double amt = 0;
                    double.TryParse(parts[7], out amt);
                    string status = parts[8];
                    
                    DateTime dt;
                    if (!DateTime.TryParse($"{dDate} {dTime}", out dt)) dt = DateTime.Now;

                    Order o = new Order(oid, dt);
                    o.OrderTotal = amt;
                    o.OrderStatus = status;
                    o.DeliveryAddress = addr;
                    
                    if (DateTime.TryParse(createdDt, out DateTime cdt)) o.OrderDateTime = cdt;

                    // Link to Customer
                    Customer? c = customerList.Find(x => x.Email == email);
                    if (c != null) c.OrderHistory.Add(o);

                        Restaurant? r = restaurantList.Find(x => x.RestaurantID == rid);
                        if (r != null)
                        {
                            if (status == "Pending" || status == "Preparing")
                            {
                                r.OrderQueue.Enqueue(o);
                            }

                            // Parse Items parts[9]
                            string itemsStr = parts[9].Replace("\"", "");
                            if (!string.IsNullOrWhiteSpace(itemsStr))
                            {
                                string[] itemPairs = itemsStr.Split('|');
                                foreach(var pair in itemPairs)
                                {
                                    string[] ip = pair.Split(',');
                                    if (ip.Length >= 2)
                                    {
                                        string iName = ip[0].Trim();
                                        int qty = 0;
                                        int.TryParse(ip[1], out qty);
                                        
                                        FoodItem? fItem = r.Menu.FoodItems.Find(f => f.ItemName == iName);
                                        if (fItem != null)
                                        {
                                            OrderedFoodItem ofi = new OrderedFoodItem(fItem, qty);
                                            if (ip.Length >= 3) ofi.Customise = ip[2].Replace(";", ","); // Restore commas
                                            o.AddOrderedFoodItem(ofi);
                                        }
                                    }
                                }
                            }
                        }
                        
                        // Payment Method
                        if (parts.Count >= 11) o.OrderPaymentMethod = parts[10];
                        // Removed SpecialRequest Logic

                        allOrders.Add(o);
                        orderRestaurantMap[o.OrderID] = rid; // Track Restaurant linkage
                    }
                }
            }
        }
    catch(Exception ex) { Console.WriteLine($"Error loading customers/orders: {ex.Message}"); }
}

// Feature 3 Done by Chong Boon
void ListRestaurantsAndMenu()
{
    Console.WriteLine("\nAll Restaurants and Menu Items");
    Console.WriteLine("==============================");
    foreach(var r in restaurantList)
    {
        Console.WriteLine($"Restaurant: {r.Name} ({r.RestaurantID})");
        foreach(var item in r.Menu.FoodItems)
        {
            Console.WriteLine($"- {item.ItemName}: {item.Description} - {item.Price:C2}");
        }
        Console.WriteLine();
    }
}

// Feature 4 Done by Jie Shen
void ListAllOrders()
{
    Console.WriteLine("\nAll Orders");
    Console.WriteLine("==========");
    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-20} {4,-10} {5,-10}", 
        "Order ID", "Customer", "Restaurant", "Delivery Date/Time", "Amount", "Status");
    Console.WriteLine("----------------------------------------------------------------------------------");

    foreach(var o in allOrders)
    {
        string cName = "Unknown";
        Customer? c = customerList.Find(x => x.OrderHistory.Exists(oh => oh.OrderID == o.OrderID));
        if (c != null) cName = c.Name;

        string rName = "Unknown"; 
        
        Restaurant? r = null;
        if (orderRestaurantMap.ContainsKey(o.OrderID))
        {
             string rid = orderRestaurantMap[o.OrderID];
             r = restaurantList.Find(x => x.RestaurantID == rid);
        }
        
        if (r != null) rName = r.Name;
        
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-20} {4,-10:C2} {5,-10}", 
             o.OrderID, cName, rName, o.DeliveryDateTime, o.OrderTotal, o.OrderStatus);
    }
}

// Feature 5 Done by Chong Boon
void CreateNewOrder()
{
    Console.WriteLine("\nCreate New Order");
    Console.WriteLine("================");
    
    // Customer
    Customer? c = null;
    while(c == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine() ?? "";
        c = customerList.Find(x => x.Email == email);
        if (c == null) Console.WriteLine("Customer not found. Please try again.");
    }

    // Restaurant
    Restaurant? r = null;
    while(r == null)
    {
        Console.Write("Enter Restaurant ID: ");
        string rid = Console.ReadLine() ?? "";
        r = restaurantList.Find(x => x.RestaurantID == rid);
        if (r == null) Console.WriteLine("Restaurant not found. Please try again.");
    }

    // Delivery Details
    DateTime dt = DateTime.MinValue;
    while(true)
    {
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string dDate = Console.ReadLine() ?? "";
        Console.Write("Enter Delivery Time (hh:mm): ");
        string dTime = Console.ReadLine() ?? "";
        
        if (DateTime.TryParse($"{dDate} {dTime}", out dt)) break;
        Console.WriteLine("Invalid date/time. Please try again.");
    }

    string addr = "";
    while(string.IsNullOrWhiteSpace(addr))
    {
        Console.Write("Enter Delivery Address: ");
        addr = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(addr)) Console.WriteLine("Address cannot be empty.");
    }

    // Add Items
    Order newOrder = new Order(nextOrderId, dt);
    newOrder.DeliveryAddress = addr;
    nextOrderId++;

    Console.WriteLine();
    Console.WriteLine("Available Food Items:");
    for(int i=0; i<r.Menu.FoodItems.Count; i++)
    {
        Console.WriteLine($"{i+1}. {r.Menu.FoodItems[i].ItemName} - {r.Menu.FoodItems[i].Price:C2}");
    }

    while(true)
    {
        Console.Write("Enter item number (0 to finish): ");
        if (!int.TryParse(Console.ReadLine(), out int choice)) 
        {
             Console.WriteLine("Invalid number.");
             continue;
        }
        if (choice == 0) break;
        
        if (choice > 0 && choice <= r.Menu.FoodItems.Count)
        {
            Console.Write("Enter quantity: ");
            if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
            {
                FoodItem item = r.Menu.FoodItems[choice-1];
                newOrder.AddOrderedFoodItem(new OrderedFoodItem(item, qty));
            }
            else Console.WriteLine("Invalid quantity.");
        }
        else Console.WriteLine("Invalid item number.");
    }

    // Special Requests
    Console.Write("Add special request? [Y/N]: ");
    if ((Console.ReadLine() ?? "").ToUpper() == "Y")
    {
        Console.Write("Enter special request: ");
        string note = Console.ReadLine() ?? "";
        foreach(var item in newOrder.OrderItems)
        {
            item.Customise = note;
        }
    }

    // Total & Payment
    double subtotal = newOrder.CalculateOrderTotal();
    double deliveryFee = 5.00; 
    double grandTotal = subtotal + deliveryFee;
    newOrder.OrderTotal = grandTotal;

    Console.WriteLine();
    Console.WriteLine($"Order Total: {subtotal:C2} + {deliveryFee:C2} (delivery) = {grandTotal:C2}");
    Console.Write("Proceed to payment? [Y/N]: ");
    if ((Console.ReadLine() ?? "").ToUpper() != "Y") return; 

    // Payment Method
    string pMethod = "";
    Console.WriteLine();
    while(true)
    {
        Console.WriteLine("Payment method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        pMethod = (Console.ReadLine() ?? "").ToUpper();
        if (pMethod == "CC" || pMethod == "PP" || pMethod == "CD") break;
        Console.WriteLine("Invalid payment method. Please try again.");
    }
    
    newOrder.OrderPaymentMethod = pMethod;
    newOrder.OrderPaid = true;

    // Finalize
    newOrder.OrderStatus = "Pending";
    c.OrderHistory.Add(newOrder);
    r.OrderQueue.Enqueue(newOrder);
    allOrders.Add(newOrder);
    orderRestaurantMap[newOrder.OrderID] = r.RestaurantID; // Track Restaurant linkage

    SaveAllOrders();
    Console.WriteLine();
    Console.WriteLine($"Order {newOrder.OrderID} created successfully! Status: Pending");
}

// Feature 6 Done by Jie Shen
void ProcessOrder()
{
    Console.WriteLine("\nProcess Order");
    Console.WriteLine("=============");
    
    // Select Restaurant
    
    Restaurant? restaurant = null;
    while(restaurant == null)
    {
        Console.Write("Enter Restaurant ID to process order from: ");
        string rid = Console.ReadLine() ?? "";
        restaurant = restaurantList.Find(x => x.RestaurantID == rid);
        if (restaurant == null) Console.WriteLine("Restaurant not found. Please try again.");
    }
    
    if (restaurant.OrderQueue.Count == 0)
    {
        Console.WriteLine("No pending orders for this restaurant.");
        return;
    }
    
    // Loop through queue using a temporary queue to rebuild
    Queue<Order> remainingOrders = new Queue<Order>();
    int originalCount = restaurant.OrderQueue.Count; // Store initial count
    
    // We cannot just use foreach because we need to modify the queue structure (dequeue/enqueue)
    // We must consume the current queue.
    while (restaurant.OrderQueue.Count > 0)
    {
        Order o = restaurant.OrderQueue.Dequeue();
        
        // Find Customer Name using robust ID lookup
        string cName = "Unknown";
        Customer? c = customerList.Find(cust => cust.OrderHistory.Exists(h => h.OrderID == o.OrderID));
        if (c == null)
        {
             // Fallback or log if needed
        }
        if (c != null) cName = c.Name;

        Console.WriteLine($"Order {o.OrderID}:");
        Console.WriteLine($"Customer: {cName}");
        Console.WriteLine("Ordered Items:");
        for(int i=0; i<o.OrderItems.Count; i++)
        {
            Console.WriteLine($"{i+1}. {o.OrderItems[i].ItemName} - {o.OrderItems[i].QtyOrdered}");
        }
        Console.WriteLine($"Delivery date/time: {o.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: {o.OrderTotal:C2}");
        Console.WriteLine($"Order Status: {o.OrderStatus}");
        
        // Action Loop
        string action = "";
        Console.WriteLine();
        while(true)
        {
            Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            action = (Console.ReadLine() ?? "").ToUpper();
            
            if (action == "C")
            {
                if (o.OrderStatus == "Pending") break;
                else Console.WriteLine("Invalid action. Only 'Pending' orders can be confirmed.");
            }
            else if (action == "R")
            {
                if (o.OrderStatus == "Pending") break;
                else Console.WriteLine("Invalid action. Only 'Pending' orders can be rejected.");
            }
            else if (action == "D")
            {
                if (o.OrderStatus == "Preparing") break;
                else Console.WriteLine("Invalid action. Only 'Preparing' orders can be delivered.");
            }
            else if (action == "S")
            {
                break; // Skip is always valid input, logic handled below
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }
        }
        
        bool keepInQueue = true; // Default to keeping unless action removes it
        
        Console.WriteLine();
        if (action == "C")
        {
             o.OrderStatus = "Preparing";
             Console.WriteLine($"Order {o.OrderID} confirmed. Status: Preparing");
             // Keep in queue
        }
        else if (action == "R")
        {
             o.OrderStatus = "Rejected";
             keepInQueue = false; // Remove from queue
             refundStack.Push(o);
             Console.WriteLine($"Order {o.OrderID} rejected. Added to Refund Stack.");
        }
        else if (action == "D")
        {
             o.OrderStatus = "Delivered";
             keepInQueue = false; // Remove from queue
             Console.WriteLine($"Order {o.OrderID} delivered.");
        }
        else if (action == "S")
        {
            if (o.OrderStatus == "Cancelled")
            {
                 keepInQueue = false; // Remove from queue (as per prompt "move on")
                 Console.WriteLine($"Cancelled order {o.OrderID} removed from queue.");
            }
            else
            {
                 Console.WriteLine("Skipped.");
                 // Keep in queue
            }
        }
        
        if (keepInQueue)
        {
            remainingOrders.Enqueue(o);
        }
        
        Console.WriteLine();
    }
    
    // Restore queue
    restaurant.OrderQueue.Clear();
    foreach(var ord in remainingOrders) restaurant.OrderQueue.Enqueue(ord);
    
    SaveAllOrders();
}

// Feature 7 Done by Chong Boon
void ModifyOrder()
{
    Console.WriteLine("\nModify Order");
    Console.WriteLine("============");
    
    Customer? c = null;
    while(c == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine() ?? "";
        c = customerList.Find(x => x.Email == email);
        if(c == null) Console.WriteLine("Customer not found. Please try again.");
    }

    Console.WriteLine("Pending Orders:");
    var pending = c.OrderHistory.FindAll(x => x.OrderStatus == "Pending");
    foreach(var p in pending) Console.WriteLine(p.OrderID);
    if (pending.Count == 0) return;

    Order? o = null;
    while(o == null)
    {
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int oid))
        {
             Console.WriteLine("Invalid ID.");
             continue;
        }
        o = pending.Find(x => x.OrderID == oid);
        if (o == null) Console.WriteLine("Order not found or not from this customer's pending list. Please try again.");
    }

    Console.WriteLine("Order Items:");
    for(int i=0; i<o.OrderItems.Count; i++)
    {
        Console.WriteLine($"{i+1}. {o.OrderItems[i]}");
    }

    Console.WriteLine("Address:");
    Console.WriteLine(o.DeliveryAddress);
    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(o.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"));


        Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
        string ch = Console.ReadLine() ?? "";
        
        // if (ch == "0") break;
        if (ch == "1")
        {
             Console.Write("Enter item number to remove (0 to cancel): ");
             int idx;
             if (int.TryParse(Console.ReadLine(), out idx) && idx > 0 && idx <= o.OrderItems.Count)
             {
                 var itemToRemove = o.OrderItems[idx-1];
                 o.RemoveOrderedFoodItem(itemToRemove);
                 Console.WriteLine("Item removed.");
                 o.OrderTotal = o.CalculateOrderTotal() + 5.00; 
                 Console.WriteLine($"New Total: {o.OrderTotal:C2}");
                 SaveAllOrders(); 
             }
             else if (idx != 0) Console.WriteLine("Invalid Item Number.");
        }
        else if (ch == "2")
        {
             Console.Write("Enter new Address: ");
             string newAddr = Console.ReadLine() ?? "";
             if (!string.IsNullOrWhiteSpace(newAddr))
             {
                 o.DeliveryAddress = newAddr;
                 Console.WriteLine($"Order {o.OrderID} updated. New Address: {o.DeliveryAddress}");
                 SaveAllOrders();
             }
        }
        else if (ch == "3")
        {
             Console.Write("Enter new Delivery Time (hh:mm): ");
             string newTime = Console.ReadLine() ?? "";
             Console.WriteLine();
             if (TimeSpan.TryParse(newTime, out TimeSpan ts))
             {
                 o.DeliveryDateTime = o.DeliveryDateTime.Date + ts;
                 Console.WriteLine($"Order {o.OrderID} updated. New Delivery Time: {o.DeliveryDateTime:HH:mm}");
                 SaveAllOrders();
             }
             else Console.WriteLine("Invalid Time.");
        }
        else Console.WriteLine("Invalid Option.");
}

// Feature 8 Done by Jie Shen
void DeleteOrder()
{
    Console.WriteLine("\nDelete Order");
    Console.WriteLine("============");
    
    Customer? c = null;
    while (c == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine() ?? "";
        c = customerList.Find(x => x.Email == email);
        if (c == null) Console.WriteLine("Customer not found. Please try again.");
    }

    // Filter Pending Orders
    var pending = c.OrderHistory.FindAll(x => x.OrderStatus == "Pending");
    
    if (pending.Count == 0)
    {
        Console.WriteLine("No pending orders found.");
        return;
    }

    Console.WriteLine("Pending Orders:");
    foreach(var p in pending)
    {
        Console.WriteLine(p.OrderID);
    }

    Order? o = null;
    while(o == null)
    {
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int oid))
        {
            Console.WriteLine("Invalid ID.");
            continue;
        }
        o = pending.Find(x => x.OrderID == oid);
        if (o == null) Console.WriteLine("Order not found or not pending. Please try again.");
    }
    
    // Display Details exactly as requested
    // "Customer: Alice Tan"
    Console.WriteLine();
    Console.WriteLine($"Customer: {c.Name}");
    Console.WriteLine("Ordered Items:");
    for(int i=0; i<o.OrderItems.Count; i++)
    {
        Console.WriteLine($"{i+1}. {o.OrderItems[i].ItemName} - {o.OrderItems[i].QtyOrdered}");
    }
    Console.WriteLine($"Delivery date/time: {o.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: {o.OrderTotal:C2}");
    Console.WriteLine($"Order Status: {o.OrderStatus}");
    
    while(true)
    {
        Console.Write("Confirm deletion? [Y/N]: ");
        string confirm = (Console.ReadLine() ?? "").ToUpper();
        
        if (confirm == "Y")
        {
            Console.WriteLine();
            // Update Status to "Cancelled"
            o.OrderStatus = "Cancelled";
            
            // Add to Refund Stack
            refundStack.Push(o);
            
            // Note: We do NOT remove from any lists (OrderHistory, allOrders, OrderQueue)
            // Feature 6 will handle removal from queue if "Skip" is selected.
            
            SaveAllOrders();
            Console.WriteLine($"Order {o.OrderID} cancelled. Refund of {o.OrderTotal:C2} processed");
            break;
        }
        else if (confirm == "N")
        {
            Console.WriteLine("Deletion cancelled.");
            break;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter Y or N.");
        }
    }
}



// Helper to save all orders to CSV
void SaveAllOrders()
{
    try
    {
        List<string> lines = new List<string>();
        // Header
        lines.Add("OrderID,MemberEmail,RestaurantID,DeliveryDate,DeliveryTime,DeliveryAddress,OrderCreated,Amount,Status,Items,PaymentMethod");
        
        foreach(var o in allOrders)
        {
             string rid = "";
             if (orderRestaurantMap.ContainsKey(o.OrderID)) rid = orderRestaurantMap[o.OrderID];
             lines.Add(FormatOrderForCSV(o, rid));
        }
        
        File.WriteAllLines("orders.csv", lines);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Error saving orders: {ex.Message}");
    }
}

void SaveQueuesAndStack()
{
    try 
    {
        // Save Queue (All restaurants' queues)
        List<string> qLines = new List<string>();
        qLines.Add("OrderID,MemberEmail,RestaurantID,DeliveryDate,DeliveryTime,DeliveryAddress,OrderCreated,Amount,Status,Items,PaymentMethod");
        
        // We write strict CSV format for orders in queue
        foreach(var r in restaurantList)
        {
            foreach(var o in r.OrderQueue)
            {
                qLines.Add(FormatOrderForCSV(o, r.RestaurantID));
            }
        }
        File.WriteAllLines("queue.csv", qLines);
        Console.WriteLine("Queue information saved to queue.csv");

        // Save Stack (Refund Stack)
        List<string> sLines = new List<string>();
        sLines.Add("OrderID,MemberEmail,RestaurantID,DeliveryDate,DeliveryTime,DeliveryAddress,OrderCreated,Amount,Status,Items,PaymentMethod");
        foreach(var o in refundStack)
        {
             // For stack, restaurant ID might be known via map
             string rid = orderRestaurantMap.ContainsKey(o.OrderID) ? orderRestaurantMap[o.OrderID] : "";
             sLines.Add(FormatOrderForCSV(o, rid));
        }
        File.WriteAllLines("stack.csv", sLines);
        Console.WriteLine("Stack information saved to stack.csv");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving queue/stack: {ex.Message}");
    }
}

string FormatOrderForCSV(Order o, string rid)
{
     Customer? c = customerList.Find(x => x.OrderHistory.Exists(oh => oh.OrderID == o.OrderID)); // Better check
     string email = c != null ? c.Email : "Unknown";
     
     string itemsStr = "";
     List<string> iList = new List<string>();
     foreach(var i in o.OrderItems) 
     {
         string cust = string.IsNullOrWhiteSpace(i.Customise) ? "" : $",{i.Customise.Replace(",", ";")}"; // Sanitize commas
         iList.Add($"{i.ItemName},{i.QtyOrdered}{cust}");
     }
     itemsStr = string.Join("|", iList);
     
     // OrderID,MemberEmail,RestaurantID,DeliveryDate,DeliveryTime,DeliveryAddress,OrderCreated,Amount,Status,Items,PaymentMethod
     return $"{o.OrderID},{Csv(email)},{Csv(rid)},{o.DeliveryDateTime:dd/MM/yyyy},{o.DeliveryDateTime:HH:mm},{Csv(o.DeliveryAddress)},{o.OrderDateTime:dd/MM/yyyy HH:mm},{o.OrderTotal},{Csv(o.OrderStatus)},\"{itemsStr}\",{Csv(o.OrderPaymentMethod)}";
}

string Csv(string s)
{
    if (s == null) return "";
    string res = s;
    if (res.Contains('"')) res = res.Replace("\"", "\"\"");
    if (res.Contains(',') || res.Contains('\n') || res.Contains('\r') || res.Contains('"'))
        return $"\"{res}\"";
    return res;
}

// Advanced Feature (a)
void BulkProcessOrders()
{
    Console.WriteLine("\nBulk Processing Unprocessed Orders...");
    int startPending = 0;
    int processedCount = 0;
    int preparedCount = 0;
    int rejectedCount = 0;

    // Identify all orders with status "Pending"
    // Also track total count for percentage calculation
    foreach(var r in restaurantList)
    {
        Queue<Order> tempQueue = new Queue<Order>();
        while(r.OrderQueue.Count > 0)
        {
            Order o = r.OrderQueue.Dequeue();
            bool keep = true;

            if (o.OrderStatus == "Pending")
            {
                startPending++;

                // Only process current day? Requirement says "for a current day". Let's restrict.
                if (o.DeliveryDateTime.Date == DateTime.Now.Date)
                {
                    processedCount++;
                    
                    // Logic: < 1 hour -> Rejected, else Preparing
                    double hoursUntilDelivery = (o.DeliveryDateTime - DateTime.Now).TotalHours;

                    if (hoursUntilDelivery < 1.0)
                    {
                        o.OrderStatus = "Rejected";
                        refundStack.Push(o);
                        rejectedCount++;
                        keep = false; // Remove from queue
                        Console.WriteLine($"[Auto-Reject] Order {o.OrderID} (Due: {o.DeliveryDateTime:HH:mm}) - Too late to prepare.");
                    }
                    else
                    {
                        o.OrderStatus = "Preparing";
                        preparedCount++;
                        Console.WriteLine($"[Auto-Confirm] Order {o.OrderID} (Due: {o.DeliveryDateTime:HH:mm}) - Set to Preparing.");
                        // Keep in queue
                    }
                }
            }
            
            if (keep) tempQueue.Enqueue(o);
        }
        
        // Restore queue
        foreach(var q in tempQueue) r.OrderQueue.Enqueue(q);
    }
    
    if (processedCount > 0) SaveAllOrders();
    
    double percentage = allOrders.Count > 0 ? ((double)processedCount / allOrders.Count) * 100 : 0;

    Console.WriteLine("\nSummary Statistics:");
    Console.WriteLine($"Total 'Pending' status orders found: {startPending}");
    Console.WriteLine($"Orders processed (Current Day): {processedCount}");
    Console.WriteLine($" - Set to Preparing: {preparedCount}");
    Console.WriteLine($" - Auto-Rejected: {rejectedCount}");
    Console.WriteLine($"Percentage of automatically processed orders against all orders: {percentage:F2}%");
}


