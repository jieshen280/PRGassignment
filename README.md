# Gruberoo Food Delivery System

## Overview
A console-based Food Delivery System built in C# for the PRG2 Assignment. This system allows for managing restaurants, menus, customers, and orders, including advanced order processing, queue management, and bonus features like special offers and favourites.

## Student Information
- **Name:** Aw Chong Boon
- **Student Number:** S10275159
- **Partner Name:** Yip Jie Shen

## Features
The application implements the following features:

### Basic Features
1. **Load Data (Startup)** (Feature 1 & 2)
   - Automatically loads restaurants, menu items, customers, and order history from CSV files.
   - Loads Special Offers and Favourite Orders (Bonus Features).
2. **List Restaurants & Menus** (Feature 3)
   - Displays all available restaurants and their food items with prices.
3. **List All Orders** (Feature 4)
   - Shows a comprehensive list of all orders associated with customers and restaurants.

### Order Management
4. **Create New Order** (Feature 5)
   - Allows users to select a customer, restaurant, and food items to place a new delivery order.
   - Includes support for special requests and delivery charges.
5. **Process Order** (Feature 6)
   - Simulate restaurant operations by processing orders from a queue.
   - Options to Confirm, Reject, Skip, or Deliver orders.
   - Rejected orders are moved to a Refund Stack.
6. **Modify Order** (Feature 7)
   - Edit pending orders: Add/Remove items, change delivery address, or update delivery time.
   - Handles price differences for modified orders.
7. **Delete Order** (Feature 8)
   - Cancel pending orders and process them for refund (moved to Refund Stack).

### Advanced Features
- **(a) Process Unspecified Orders:** Bulk process pending orders for the current day based on delivery time (Auto-Reject if < 1 hour, else Auto-Confirm).
- **(b) Display Total Receipts:** Calculate and display total sales and refunds per restaurant, and Gruberoo's total earnings from delivery fees.

### Bonus Features
- **(c) Special Offers:** Apply promotional codes (e.g., discounts, free delivery, buy-one-get-one-free) to pending orders.
- **(d) Favourite Orders:** Save frequently ordered items as "Favourites" and quickly reorder them.

## Data Files
- `restaurants.csv`: Restaurant details (ID, Name, Email).
- `fooditems.csv`: Menu items linked to restaurants.
- `customers.csv`: Registered customer data.
- `orders.csv`: History of all orders.
- `queue.csv`: Current state of order queues (saved on exit).
- `stack.csv`: Current state of refund stack (saved on exit, not explicitly used but part of requirements).
- `specialoffers.csv`: Promotional offers for restaurants.
- `favourites.csv`: Saved customer favourite orders.

## How to Run
1. Ensure all CSV data files are in the same directory as the executable (or project root).
2. Open the project in Visual Studio or Terminal.
3. Run the application:
   ```bash
   dotnet run
   ```
