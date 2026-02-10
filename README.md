# Gruberoo Food Delivery System

## Overview
A console-based Food Delivery System built in C# for the PRG2 Assignment. This system allows for managing restaurants, menus, customers, and orders, including advanced order processing and queue management.

## Student Information
- **Name:** Aw Chong Boon
- **Student Number:** S10275159
- **Partner Name:** Yip Jie Shen

## Features
The application implements the following features:

### Basic Features
1. **Load Data (Startup)**
   - Automatically loads restaurants, menu items, customers, and order history from CSV files.
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
   - Edit pending orders: Remove items, change delivery address, or update delivery time.
7. **Delete Order** (Feature 8)
   - Cancel pending orders and process them for refund (moved to Refund Stack).

### Advanced Features
- **Queue & Stack Persistence:** Automatically saves the current state of order queues and the refund stack to `queue.csv` and `stack.csv` upon exit.
- **Data Persistence:** Updates `orders.csv` dynamically as orders are created or modified.

## Data Files
- `restaurants.csv`: Restaurant details (ID, Name, Email).
- `fooditems.csv`: Menu items linked to restaurants.
- `customers.csv`: Registered customer data.
- `orders.csv`: History of all orders.
- `queue.csv` / `stack.csv`: System state snapshots.

## How to Run
1. Ensure the CSV data files are in the same directory as the executable (or project root).
2. Open the project in Visual Studio or Terminal.
3. Run the application:
   ```bash
   dotnet run
   ```
