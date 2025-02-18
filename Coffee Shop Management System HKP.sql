-- User Table
CREATE TABLE [DBO].[User] (
    UserID INT NOT NULL PRIMARY KEY IDENTITY,
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    Address VARCHAR(100) NOT NULL,
    IsActive BIT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL
);

-- Product Table
CREATE TABLE [DBO].[Product] (
    ProductID INT NOT NULL PRIMARY KEY IDENTITY,
    ProductName VARCHAR(100) NOT NULL,
    ProductPrice DECIMAL(10,2) NOT NULL,
    ProductCode VARCHAR(100) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (UserID) REFERENCES [DBO].[User](UserID)
);
ALTER TABLE [DBO].[Product]
ADD [Category] NVARCHAR(255) NULL,
    [StockQuantity] INT NOT NULL DEFAULT 0;

UPDATE [DBO].[Product]
SET [StockQuantity] = 100;

WITH CategorizedProducts AS (
    SELECT 
        ProductID,
        CASE 
            WHEN ProductID % 5 = 0 THEN 'Specialty Drinks' -- Divisible by 5
            WHEN ProductID % 7 = 0 THEN 'Hot Drinks'       -- Divisible by 7
            WHEN ProductID = 2 THEN 'Grocery'             -- 2 is a prime number
            WHEN ProductID > 1 AND NOT EXISTS ( -- Check for prime numbers
                SELECT 1
                FROM master.dbo.spt_values v
                WHERE v.type = 'P' 
                  AND v.number BETWEEN 2 AND SQRT(ProductID)
                  AND ProductID % v.number = 0
            ) THEN 'Grocery'
            WHEN ProductID % 2 = 0 THEN 'Beverages'       -- Even numbers
            ELSE 'Food & Drink'                          -- Odd numbers
        END AS Category
    FROM [DBO].[Product]
)
UPDATE P
SET [Category] = C.Category
FROM [DBO].[Product] P
JOIN CategorizedProducts C ON P.ProductID = C.ProductID;

SELECT * FROM Product
select Product.Category from Product group by Product.Category

-- Order Table
CREATE TABLE [DBO].[Order] (
    OrderID INT NOT NULL PRIMARY KEY IDENTITY,
    OrderNumber VARCHAR(50) NOT NULL,
    OrderDate DATETIME NOT NULL,
    CustomerID INT NOT NULL,
    PaymentMode VARCHAR(100) NULL,
    TotalAmount DECIMAL(10,2) NULL,
    ShippingAddress VARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (CustomerID) REFERENCES [DBO].[Customer](CustomerID),
    FOREIGN KEY (UserID) REFERENCES [DBO].[User](UserID)
);
ALTER TABLE [DBO].[Order]
ADD [Status] NVARCHAR(50) NULL DEFAULT ('Pending');

-- OrderDetail Table
CREATE TABLE [DBO].[OrderDetail] (
    OrderDetailID INT NOT NULL PRIMARY KEY IDENTITY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (OrderID) REFERENCES [DBO].[Order](OrderID),
    FOREIGN KEY (ProductID) REFERENCES [DBO].[Product](ProductID),
    FOREIGN KEY (UserID) REFERENCES [DBO].[User](UserID)
);

-- Bills Table
CREATE TABLE [DBO].[Bills] (
    BillID INT NOT NULL PRIMARY KEY IDENTITY,
    BillNumber VARCHAR(100) NOT NULL,
    BillDate DATETIME NOT NULL,
    OrderID INT NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    Discount DECIMAL(10,2) NULL,
    NetAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (OrderID) REFERENCES [DBO].[Order](OrderID),
    FOREIGN KEY (UserID) REFERENCES [DBO].[User](UserID)
);

-- Customer Table
CREATE TABLE [DBO].[Customer] (
    CustomerID INT NOT NULL PRIMARY KEY IDENTITY,
    CustomerName VARCHAR(100) NOT NULL,
    HomeAddress VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    GST_NO VARCHAR(15) NOT NULL,
    CityName VARCHAR(100) NOT NULL,
    PinCode VARCHAR(15) NOT NULL,
    NetAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ModifiedAt DATETIME NULL,
    FOREIGN KEY (UserID) REFERENCES [DBO].[User](UserID)
);

SELECT * FROM Product
SELECT * FROM [User]
SELECT * FROM [Order]
SELECT * FROM OrderDetail
SELECT * FROM Bills
SELECT * FROM Customer

-- Store Procedure Of Coffee Shop Management System --

-- Store Procedure Of Product Table

-- 1.Select All 
ALTER PROCEDURE [dbo].[PR_LOC_Product_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Product].[ProductID],
        [dbo].[Product].[ProductName],
        [dbo].[Product].[ProductPrice],
        [dbo].[Product].[ProductCode],
        [dbo].[Product].[Description],
        [dbo].[Product].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Product].[CreatedAt],       -- Added CreatedAt field
        [dbo].[Product].[ModifiedAt]       -- Added ModifiedAt field
    FROM 
        [dbo].[Product]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Product].[UserID] = [dbo].[User].[UserID]
    ORDER BY 
        [dbo].[Product].[ProductName]    -- Ordering by ProductName
END

-- EXEC [dbo].[PR_Product_SelectAll]

-- 2.Select By PK 
ALTER PROCEDURE [dbo].[PR_LOC_Product_SelectByPK]
    @ProductID INT
AS
BEGIN
    SELECT 
        [dbo].[Product].[ProductID],
        [dbo].[Product].[ProductName],
        [dbo].[Product].[ProductPrice],
        [dbo].[Product].[ProductCode],
        [dbo].[Product].[Description],
        [dbo].[Product].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Product].[CreatedAt],       -- Added CreatedAt field
        [dbo].[Product].[ModifiedAt]       -- Added ModifiedAt field
    FROM 
        [dbo].[Product]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Product].[UserID] = [dbo].[User].[UserID]
    WHERE 
        [dbo].[Product].[ProductID] = @ProductID
END
-- EXEC [dbo].[PR_Product_SelectByPK] 1

-- 3.Insert
ALTER PROCEDURE [dbo].[PR_LOC_Product_Insert]
    @ProductName VARCHAR(100),
    @ProductPrice DECIMAL(10,2),
    @ProductCode VARCHAR(100),
    @Description VARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Product]
    (
        [ProductName],
        [ProductPrice],
        [ProductCode],
        [Description],
        [UserID],
        [CreatedAt]       -- Set CreatedAt to the current time
    )
    VALUES
    (
        @ProductName,
        @ProductPrice,
        @ProductCode,
        @Description,
        @UserID,
        GETDATE()        -- Set CreatedAt to the current time
    )
END
-- EXEC [dbo].[PR_Product_Insert] Give Product Details

-- 4.Update By PK
ALTER PROCEDURE [dbo].[PR_LOC_Product_UpdateByPK]
    @ProductID INT,
    @ProductName VARCHAR(100),
    @ProductPrice DECIMAL(10,2),
    @ProductCode VARCHAR(100),
    @Description VARCHAR(100),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Product]
    SET 
        [ProductName] = @ProductName,
        [ProductPrice] = @ProductPrice,
        [ProductCode] = @ProductCode,
        [Description] = @Description,
        [UserID] = @UserID,
        [ModifiedAt] = GETDATE()      -- Set ModifiedAt to the current time
    WHERE 
        [dbo].[Product].[ProductID] = @ProductID
END
-- EXEC [dbo].[PR_Product_UpdateByPK] Give Product Details

-- 5.Delete By PK
ALTER PROCEDURE [dbo].[PR_LOC_Product_DeleteByPK]
    @ProductID INT
AS 
BEGIN
    DELETE FROM [dbo].[Product]
    WHERE 
        [dbo].[Product].[ProductID] = @ProductID
END
-- EXEC [dbo].[PR_Product_DeleteByPK] 1

--------------------------------------------------------------------------------------------------
-- Store Procedure Of User Table

-- 1.Select All 
ALTER PROCEDURE [dbo].[PR_LOC_User_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID],
        [dbo].[User].[UserName],
        [dbo].[User].[Email],
        [dbo].[User].[Password],
        [dbo].[User].[MobileNo],
        [dbo].[User].[Address],
        [dbo].[User].[IsActive],
        [dbo].[User].[CreatedAt],       -- Added CreatedAt field
        [dbo].[User].[ModifiedAt]       -- Added ModifiedAt field
    FROM 
        [dbo].[User]
    ORDER BY 
        [dbo].[User].[UserName]        -- Ordering by UserName
END
-- EXEC [dbo].[PR_User_SelectAll]

-- 2.Select By PK 
ALTER PROCEDURE [dbo].[PR_LOC_User_SelectByPK]
    @UserID INT
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID],
        [dbo].[User].[UserName],
        [dbo].[User].[Email],
        [dbo].[User].[Password],
        [dbo].[User].[MobileNo],
        [dbo].[User].[Address],
        [dbo].[User].[IsActive],
        [dbo].[User].[CreatedAt],       -- Added CreatedAt field
        [dbo].[User].[ModifiedAt]       -- Added ModifiedAt field
    FROM 
        [dbo].[User]
    WHERE 
        [dbo].[User].[UserID] = @UserID
END
-- EXEC [dbo].[PR_User_SelectByPK] 1

-- 3.Insert
ALTER PROCEDURE [dbo].[PR_LOC_User_Insert]
    @UserName VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(100),
    @MobileNo VARCHAR(15),
    @Address VARCHAR(100),
    @IsActive BIT
AS
BEGIN
    INSERT INTO [dbo].[User]
    (
        [UserName],
        [Email],
        [Password],
        [MobileNo],
        [Address],
        [IsActive],
        [CreatedAt]       -- Set CreatedAt to the current time
    )
    VALUES
    (
        @UserName,
        @Email,
        @Password,
        @MobileNo,
        @Address,
        @IsActive,
        GETDATE()          -- Set CreatedAt to the current time
    )
END
-- EXEC [dbo].[PR_User_Insert] 'John Doe', 'john.doe@example.com', 'password123', '1234567890', '123 Main St', 1

-- 4.Update By PK
ALTER PROCEDURE [dbo].[PR_LOC_User_UpdateByPK]
    @UserID INT,
    @UserName VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(100),
    @MobileNo VARCHAR(15),
    @Address VARCHAR(100),
    @IsActive BIT
AS
BEGIN
    UPDATE [dbo].[User]
    SET 
        [UserName] = @UserName,
        [Email] = @Email,
        [Password] = @Password,
        [MobileNo] = @MobileNo,
        [Address] = @Address,
        [IsActive] = @IsActive,
        [ModifiedAt] = GETDATE()      -- Set ModifiedAt to the current time
    WHERE 
        [UserID] = @UserID
END
-- EXEC [dbo].[PR_User_UpdateByPK] 1, 'John Doe', 'john.doe@example.com', 'newpassword123', '9876543210', '456 Another St', 1

-- 5.Delete By PK
CREATE PROCEDURE [dbo].[PR_LOC_User_DeleteByPK]
    @UserID INT
AS 
BEGIN
    DELETE FROM [dbo].[User]
    WHERE [dbo].[User].[UserID] = @UserID
END
-- EXEC [dbo].[PR_User_DeleteByPK] 1

SELECT * FROM [User]
--------------------------------------------------------------------------------------------------
-- Store Procedure Of Order Table

-- 1. Select All Orders
ALTER PROCEDURE [dbo].[PR_LOC_Order_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Order].[OrderID],
        [dbo].[Order].[OrderNumber],
        [dbo].[Order].[OrderDate],
        [dbo].[Order].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Order].[PaymentMode],
        [dbo].[Order].[TotalAmount],
        [dbo].[Order].[ShippingAddress],
        [dbo].[Order].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Order].[CreatedAt],       -- Added CreatedAt field
        [dbo].[Order].[ModifiedAt]       -- Added ModifiedAt field
    FROM 
        [dbo].[Order]
    INNER JOIN [dbo].[User]
        ON [dbo].[Order].[UserID] = [dbo].[User].[UserID]
    INNER JOIN [dbo].[Customer]
        ON [dbo].[Customer].[CustomerID] = [dbo].[Order].[CustomerID]
END
-- EXEC [dbo].[PR_Order_SelectAll]

-- 2. Select Order by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Order_SelectByPK]
    @OrderID INT
AS
BEGIN
    SELECT 
        [dbo].[Order].[OrderID],
        [dbo].[Order].[OrderNumber],
        [dbo].[Order].[OrderDate],
        [dbo].[Order].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Order].[PaymentMode],
        [dbo].[Order].[TotalAmount],
        [dbo].[Order].[ShippingAddress],
        [dbo].[Order].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Order].[CreatedAt],       -- Added CreatedAt field
        [dbo].[Order].[ModifiedAt]       -- Added ModifiedAt field
    FROM [dbo].[Order]
    INNER JOIN [dbo].[User]
        ON [dbo].[Order].[UserID] = [dbo].[User].[UserID]
    INNER JOIN [dbo].[Customer]
        ON [dbo].[Customer].[CustomerID] = [dbo].[Order].[CustomerID]
    WHERE [dbo].[Order].[OrderID] = @OrderID
END
-- EXEC [dbo].[PR_Order_SelectByPK] 1

-- 3. Insert a New Order
ALTER PROCEDURE [dbo].[PR_LOC_Order_Insert]
    @OrderDate DATETIME,
    @OrderNumber VARCHAR(50),
    @CustomerID INT,
    @PaymentMode VARCHAR(100) = NULL,
    @TotalAmount DECIMAL(10,2) = NULL,
    @ShippingAddress VARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Order]
    (
        [dbo].[Order].[OrderDate],
        [dbo].[Order].[OrderNumber],
        [dbo].[Order].[CustomerID],
        [dbo].[Order].[PaymentMode],
        [dbo].[Order].[TotalAmount],
        [dbo].[Order].[ShippingAddress],
        [dbo].[Order].[UserID],
        [dbo].[Order].[CreatedAt]  -- Automatically set when inserting
    )
    VALUES
    (
        @OrderDate,
        @OrderNumber,
        @CustomerID,
        @PaymentMode,
        @TotalAmount,
        @ShippingAddress,
        @UserID,
        GETDATE()   -- Automatically setting CreatedAt to current timestamp
    )
END
-- EXEC [dbo].[PR_Order_Insert] '2023-08-09 12:00:00', 1, 'Credit Card', 100.00, '123 Shipping St', 1

-- 4. Update Order by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Order_UpdateByPK]
    @OrderID INT,
    @OrderNumber VARCHAR(50),
    @OrderDate DATETIME,
    @CustomerID INT,
    @PaymentMode VARCHAR(100) = NULL,
    @TotalAmount DECIMAL(10,2) = NULL,
    @ShippingAddress VARCHAR(100),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Order]
    SET 
        [dbo].[Order].[OrderDate] = @OrderDate,
        [dbo].[Order].[OrderNumber] = @OrderNumber,
        [dbo].[Order].[CustomerID] = @CustomerID,
        [dbo].[Order].[PaymentMode] = @PaymentMode,
        [dbo].[Order].[TotalAmount] = @TotalAmount,
        [dbo].[Order].[ShippingAddress] = @ShippingAddress,
        [dbo].[Order].[UserID] = @UserID,
        [dbo].[Order].[ModifiedAt] = GETDATE()  -- Updating ModifiedAt to current timestamp
    WHERE [dbo].[Order].[OrderID] = @OrderID
END
-- EXEC [dbo].[PR_Order_UpdateByPK] 1, '2023-08-09 12:00:00', 1, 'Debit Card', 120.00, '456 New St', 2

-- 5. Delete Order by Primary Key
CREATE PROCEDURE [dbo].[PR_LOC_Order_DeleteByPK]
    @OrderID INT
AS 
BEGIN
    DELETE FROM [dbo].[Order]
    WHERE [dbo].[Order].[OrderID] = @OrderID
END
-- EXEC [dbo].[PR_Order_DeleteByPK] 1

--------------------------------------------------------------------------------------------------
-- Store Procedure Of Order Details Table

-- 1. Select All Order Details
ALTER PROCEDURE [dbo].[PR_LOC_OrderDetail_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[OrderDetail].[OrderDetailID],
        [dbo].[OrderDetail].[OrderID],
        [dbo].[Order].[OrderNumber],  -- Added OrderNumber
        [dbo].[OrderDetail].[ProductID],
        [dbo].[Product].[ProductName],
        [dbo].[OrderDetail].[Quantity],
        [dbo].[OrderDetail].[Amount],
        [dbo].[OrderDetail].[TotalAmount],
        [dbo].[OrderDetail].[UserID],
        [dbo].[User].[UserName],
        [dbo].[OrderDetail].[CreatedAt],   -- CreatedAt field
        [dbo].[OrderDetail].[ModifiedAt]   -- ModifiedAt field
    FROM [dbo].[OrderDetail]
    INNER JOIN [dbo].[Order]
        ON [dbo].[OrderDetail].[OrderID] = [dbo].[Order].[OrderID]  -- Join with Order table to get OrderNumber
    INNER JOIN [dbo].[Product]
        ON [dbo].[OrderDetail].[ProductID] = [dbo].[Product].[ProductID]
    INNER JOIN [dbo].[User]
        ON [dbo].[OrderDetail].[UserID] = [dbo].[User].[UserID]
END
-- EXEC [dbo].[PR_OrderDetail_SelectAll]

-- 2. Select Order Detail by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_OrderDetail_SelectByPK]
    @OrderDetailID INT
AS
BEGIN
    SELECT 
        [dbo].[OrderDetail].[OrderDetailID],
        [dbo].[OrderDetail].[OrderID],
        [dbo].[Order].[OrderNumber],  -- Added OrderNumber
        [dbo].[OrderDetail].[ProductID],
        [dbo].[Product].[ProductName],
        [dbo].[OrderDetail].[Quantity],
        [dbo].[OrderDetail].[Amount],
        [dbo].[OrderDetail].[TotalAmount],
        [dbo].[OrderDetail].[UserID],
        [dbo].[User].[UserName],
        [dbo].[OrderDetail].[CreatedAt],   -- CreatedAt field
        [dbo].[OrderDetail].[ModifiedAt]   -- ModifiedAt field
    FROM [dbo].[OrderDetail]
    INNER JOIN [dbo].[Order]
        ON [dbo].[OrderDetail].[OrderID] = [dbo].[Order].[OrderID]  -- Join with Order table to get OrderNumber
    INNER JOIN [dbo].[Product]
        ON [dbo].[OrderDetail].[ProductID] = [dbo].[Product].[ProductID]
    INNER JOIN [dbo].[User]
        ON [dbo].[OrderDetail].[UserID] = [dbo].[User].[UserID]
    WHERE [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID
END
-- EXEC [dbo].[PR_OrderDetail_SelectByPK] 1

-- 3. Insert a New Order Detail
ALTER PROCEDURE [dbo].[PR_LOC_OrderDetail_Insert]
    @OrderID INT,
    @ProductID INT,
    @Quantity INT,
    @Amount DECIMAL(10,2),
    @TotalAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[OrderDetail]
    (
        [dbo].[OrderDetail].[OrderID],
        [dbo].[OrderDetail].[ProductID],
        [dbo].[OrderDetail].[Quantity],
        [dbo].[OrderDetail].[Amount],
        [dbo].[OrderDetail].[TotalAmount],
        [dbo].[OrderDetail].[UserID],
        [dbo].[OrderDetail].[CreatedAt]  -- Automatically set when inserting
    )
    VALUES
    (
        @OrderID,
        @ProductID,
        @Quantity,
        @Amount,
        @TotalAmount,
        @UserID,
        GETDATE()   -- Automatically setting CreatedAt to current timestamp
    )
END
-- EXEC [dbo].[PR_OrderDetail_Insert] 1, 1, 2, 50.00, 100.00, 1

-- 4. Update Order Detail by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_OrderDetail_UpdateByPK]
    @OrderDetailID INT,
    @OrderID INT,
    @ProductID INT,
    @Quantity INT,
    @Amount DECIMAL(10,2),
    @TotalAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[OrderDetail]
    SET 
        [dbo].[OrderDetail].[OrderID] = @OrderID,
        [dbo].[OrderDetail].[ProductID] = @ProductID,
        [dbo].[OrderDetail].[Quantity] = @Quantity,
        [dbo].[OrderDetail].[Amount] = @Amount,
        [dbo].[OrderDetail].[TotalAmount] = @TotalAmount,
        [dbo].[OrderDetail].[UserID] = @UserID,
        [dbo].[OrderDetail].[ModifiedAt] = GETDATE()  -- Updating ModifiedAt to current timestamp
    WHERE [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID
END
-- EXEC [dbo].[PR_OrderDetail_UpdateByPK] 1, 1, 2, 3, 60.00, 180.00, 1

-- 5. Delete Order Detail by Primary Key
CREATE PROCEDURE [dbo].[PR_LOC_OrderDetail_DeleteByPK]
    @OrderDetailID INT
AS 
BEGIN
    DELETE FROM [dbo].[OrderDetail]
    WHERE [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID
END
-- EXEC [dbo].[PR_OrderDetail_DeleteByPK] 1

--------------------------------------------------------------------------------------------------
-- Store Procedure Of Bills Table

-- 1. Select All Bills
ALTER PROCEDURE [dbo].[PR_LOC_Bills_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Bills].[BillID],
        [dbo].[Bills].[BillNumber],
        [dbo].[Bills].[BillDate],
        [dbo].[Bills].[OrderID],
        [dbo].[Bills].[TotalAmount],
        [dbo].[Bills].[Discount],
        [dbo].[Bills].[NetAmount],
        [dbo].[Bills].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Bills].[CreatedAt],  -- Added CreatedAt field
        [dbo].[Bills].[ModifiedAt]  -- Added ModifiedAt field
    FROM [dbo].[Bills]
    INNER JOIN [dbo].[User]
        ON [dbo].[Bills].[UserID] = [dbo].[User].[UserID]
END
-- EXEC [dbo].[PR_Bills_SelectAll]

-- 2. Select Bill by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Bills_SelectByPK]
    @BillID INT
AS
BEGIN
    SELECT 
        [dbo].[Bills].[BillID],
        [dbo].[Bills].[BillNumber],
        [dbo].[Bills].[BillDate],
        [dbo].[Bills].[OrderID],
        [dbo].[Bills].[TotalAmount],
        [dbo].[Bills].[Discount],
        [dbo].[Bills].[NetAmount],
        [dbo].[Bills].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Bills].[CreatedAt],  -- Added CreatedAt field
        [dbo].[Bills].[ModifiedAt]  -- Added ModifiedAt field
    FROM [dbo].[Bills]
    INNER JOIN [dbo].[User]
        ON [dbo].[Bills].[UserID] = [dbo].[User].[UserID]
    WHERE [dbo].[Bills].[BillID] = @BillID
END
-- EXEC [dbo].[PR_Bills_SelectByPK] 1

-- 3. Insert a New Bill
ALTER PROCEDURE [dbo].[PR_LOC_Bills_Insert]
    @BillNumber VARCHAR(100),
    @BillDate DATETIME,
    @OrderID INT,
    @TotalAmount DECIMAL(10,2),
    @Discount DECIMAL(10,2) = NULL,
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Bills]
    (
        [dbo].[Bills].[BillNumber],
        [dbo].[Bills].[BillDate],
        [dbo].[Bills].[OrderID],
        [dbo].[Bills].[TotalAmount],
        [dbo].[Bills].[Discount],
        [dbo].[Bills].[NetAmount],
        [dbo].[Bills].[UserID],
        [dbo].[Bills].[CreatedAt]  -- Automatically set CreatedAt to current timestamp
    )
    VALUES
    (
        @BillNumber,
        @BillDate,
        @OrderID,
        @TotalAmount,
        ISNULL(@Discount, 0),
        @NetAmount,
        @UserID,
        GETDATE()  -- Sets CreatedAt to current timestamp
    )
END
-- EXEC [dbo].[PR_Bills_Insert] 'BILL-2023-001', '2023-08-09 12:00:00', 1, 500.00, 50.00, 450.00, 1

-- 4. Update Bill by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Bills_UpdateByPK]
    @BillID INT,
    @BillNumber VARCHAR(100),
    @BillDate DATETIME,
    @OrderID INT,
    @TotalAmount DECIMAL(10,2),
    @Discount DECIMAL(10,2) = NULL,
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Bills]
    SET 
        [dbo].[Bills].[BillNumber] = @BillNumber,
        [dbo].[Bills].[BillDate] = @BillDate,
        [dbo].[Bills].[OrderID] = @OrderID,
        [dbo].[Bills].[TotalAmount] = @TotalAmount,
        [dbo].[Bills].[Discount] = ISNULL(@Discount, 0),
        [dbo].[Bills].[NetAmount] = @NetAmount,
        [dbo].[Bills].[UserID] = @UserID,
        [dbo].[Bills].[ModifiedAt] = GETDATE()  -- Automatically set ModifiedAt to current timestamp
    WHERE [dbo].[Bills].[BillID] = @BillID
END
-- EXEC [dbo].[PR_Bills_UpdateByPK] 1, 'BILL-2023-002', '2023-08-10 12:00:00', 2, 600.00, 60.00, 540.00, 2

-- 5. Delete Bill by Primary Key
CREATE PROCEDURE [dbo].[PR_LOC_Bills_DeleteByPK]
    @BillID INT
AS 
BEGIN
    DELETE FROM [dbo].[Bills]
    WHERE [dbo].[Bills].[BillID] = @BillID
END
-- EXEC [dbo].[PR_Bills_DeleteByPK] 1

--------------------------------------------------------------------------------------------------
-- Store Procedure Of Customer Table

-- 1. Select All Customers
SELECT * FROM [Order]
EXEC [PR_LOC_Customer_SelectAll]
ALTER PROCEDURE [dbo].[PR_LOC_Customer_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Customer].[HomeAddress],
        [dbo].[Customer].[Email],
        [dbo].[Customer].[MobileNo],
        [dbo].[Customer].[GST_NO],
        [dbo].[Customer].[CityName],
        [dbo].[Customer].[PinCode],
        [dbo].[Customer].[NetAmount],
        [dbo].[Customer].[UserID],
        [dbo].[User].[UserName],
        COUNT([dbo].[Order].[OrderID]) AS OrderCount,
        [dbo].[Customer].[CreatedAt],  -- Added CreatedAt field
        [dbo].[Customer].[ModifiedAt]  -- Added ModifiedAt field
    FROM 
        [dbo].[Customer]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Customer].[UserID] = [dbo].[User].[UserID]
    LEFT JOIN 
        [dbo].[Order] ON [dbo].[Customer].[CustomerID] = [dbo].[Order].[CustomerID]
    GROUP BY 
        [dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Customer].[HomeAddress],
        [dbo].[Customer].[Email],
        [dbo].[Customer].[MobileNo],
        [dbo].[Customer].[GST_NO],
        [dbo].[Customer].[CityName],
        [dbo].[Customer].[PinCode],
        [dbo].[Customer].[NetAmount],
        [dbo].[Customer].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Customer].[CreatedAt],
        [dbo].[Customer].[ModifiedAt]
END

-- EXEC [dbo].[PR_Customer_SelectAll]

-- 2. Select Customer by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Customer_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Customer].[HomeAddress],
        [dbo].[Customer].[Email],
        [dbo].[Customer].[MobileNo],
        [dbo].[Customer].[GST_NO],
        [dbo].[Customer].[CityName],
        [dbo].[Customer].[PinCode],
        [dbo].[Customer].[NetAmount],
        [dbo].[Customer].[UserID],
        [dbo].[User].[UserName],
        COUNT([dbo].[Order].[OrderID]) AS OrderCount,
        [dbo].[Customer].[CreatedAt],  -- Added CreatedAt field
        [dbo].[Customer].[ModifiedAt]  -- Added ModifiedAt field
    FROM 
        [dbo].[Customer]
    INNER JOIN 
        [dbo].[User] ON [dbo].[Customer].[UserID] = [dbo].[User].[UserID]
    LEFT JOIN 
        [dbo].[Order] ON [dbo].[Customer].[CustomerID] = [dbo].[Order].[CustomerID]
    GROUP BY 
        [dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName],
        [dbo].[Customer].[HomeAddress],
        [dbo].[Customer].[Email],
        [dbo].[Customer].[MobileNo],
        [dbo].[Customer].[GST_NO],
        [dbo].[Customer].[CityName],
        [dbo].[Customer].[PinCode],
        [dbo].[Customer].[NetAmount],
        [dbo].[Customer].[UserID],
        [dbo].[User].[UserName],
        [dbo].[Customer].[CreatedAt],
        [dbo].[Customer].[ModifiedAt]
END

-- EXEC [dbo].[PR_Customer_SelectByPK] 1

-- 3. Insert a New Customer
ALTER PROCEDURE [dbo].[PR_LOC_Customer_Insert]
    @CustomerName VARCHAR(100),
    @HomeAddress VARCHAR(100),
    @Email VARCHAR(100),
    @MobileNo VARCHAR(15),
    @GST_NO VARCHAR(15),
    @CityName VARCHAR(100),
    @PinCode VARCHAR(15),
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Customer]
    (
        [dbo].[Customer].[CustomerName],
        [dbo].[Customer].[HomeAddress],
        [dbo].[Customer].[Email],
        [dbo].[Customer].[MobileNo],
        [dbo].[Customer].[GST_NO],
        [dbo].[Customer].[CityName],
        [dbo].[Customer].[PinCode],
        [dbo].[Customer].[NetAmount],
        [dbo].[Customer].[UserID],
        [dbo].[Customer].[CreatedAt]  -- Automatically set CreatedAt to current timestamp
    )
    VALUES
    (
        @CustomerName,
        @HomeAddress,
        @Email,
        @MobileNo,
        @GST_NO,
        @CityName,
        @PinCode,
        @NetAmount,
        @UserID,
        GETDATE()  -- Sets CreatedAt to current timestamp
    )
END
-- EXEC [dbo].[PR_Customer_Insert] 'John Doe', '123 Main St', 'john.doe@example.com', '1234567890', 'GST1234567', 'New York', '10001', 1000.00, 1

-- 4. Update Customer by Primary Key
ALTER PROCEDURE [dbo].[PR_LOC_Customer_UpdateByPK]
    @CustomerID INT,
    @CustomerName VARCHAR(100),
    @HomeAddress VARCHAR(100),
    @Email VARCHAR(100),
    @MobileNo VARCHAR(15),
    @GST_NO VARCHAR(15),
    @CityName VARCHAR(100),
    @PinCode VARCHAR(15),
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Customer]
    SET 
        [dbo].[Customer].[CustomerName] = @CustomerName,
        [dbo].[Customer].[HomeAddress] = @HomeAddress,
        [dbo].[Customer].[Email] = @Email,
        [dbo].[Customer].[MobileNo] = @MobileNo,
        [dbo].[Customer].[GST_NO] = @GST_NO,
        [dbo].[Customer].[CityName] = @CityName,
        [dbo].[Customer].[PinCode] = @PinCode,
        [dbo].[Customer].[NetAmount] = @NetAmount,
        [dbo].[Customer].[UserID] = @UserID,
        [dbo].[Customer].[ModifiedAt] = GETDATE()  -- Automatically set ModifiedAt to current timestamp
    WHERE [dbo].[Customer].[CustomerID] = @CustomerID
END
-- EXEC [dbo].[PR_Customer_UpdateByPK] 1, 'John Doe', '456 Another St', 'john.doe@example.com', '9876543210', 'GST7654321', 'Los Angeles', '90001', 1500.00, 2

-- 5. Delete Customer by Primary Key
CREATE PROCEDURE [dbo].[PR_LOC_Customer_DeleteByPK]
    @CustomerID INT
AS 
BEGIN
    DELETE FROM [dbo].[Customer]
    WHERE [dbo].[Customer].[CustomerID] = @CustomerID
END
-- EXEC [dbo].[PR_Customer_DeleteByPK] 1

-- DropDown Procedure
CREATE PROCEDURE [dbo].[PR_LOC_User_DropDown]
AS
BEGIN
    SELECT
		[dbo].[User].[UserID],
        [dbo].[User].[UserName]
    FROM
        [dbo].[User]
	ORDER BY [dbo].[User].[UserName] 
END

CREATE PROCEDURE [dbo].[PR_LOC_Product_DropDown]
AS
BEGIN
    SELECT
		[dbo].[Product].[ProductID],
        [dbo].[Product].[ProductName]
    FROM
        [dbo].[Product]
	ORDER BY [dbo].[Product].[ProductName] 
END

CREATE PROCEDURE [dbo].[PR_LOC_Customer_DropDown]
AS
BEGIN
    SELECT
		[dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName]
    FROM
        [dbo].[Customer]
	ORDER BY [dbo].[Customer].[CustomerName]
END

CREATE PROCEDURE [dbo].[PR_LOC_Order_DropDown]
AS
BEGIN
    SELECT
		[dbo].[Order].[OrderID],
        [dbo].[Order].[OrderNumber]
    FROM
        [dbo].[Order]
	ORDER BY [dbo].[Order].[OrderNumber]
END

--Register & Login
ALTER PROCEDURE [dbo].[PR_User_Register]
    @UserName NVARCHAR(50),
    @Password NVARCHAR(50),
    @Email NVARCHAR(500),
    @MobileNo VARCHAR(50),
    @Address VARCHAR(50)
AS
BEGIN
    INSERT INTO [dbo].[User]
    (
        [UserName],
        [Password],
        [Email],
        [MobileNo],
        [Address],
		[IsActive]
    )
    VALUES
    (
        @UserName,
        @Password,
        @Email,
        @MobileNo,
        @Address,
		'true'
    );
END

ALTER PROCEDURE [dbo].[PR_User_Login]
    @UserName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID], 
        [dbo].[User].[UserName], 
        [dbo].[User].[MobileNo], 
        [dbo].[User].[Email], 
        [dbo].[User].[Password],
        [dbo].[User].[Address]
    FROM 
        [dbo].[User] 
    WHERE 
        [dbo].[User].[UserName] = @UserName 
        AND [dbo].[User].[Password] = @Password 
		AND [dbo].[User].[IsActive] = 'TRUE';
END

SELECT * FROM [dbo].[User]
DROP PROC [dbo].[PR_User_Login]

CREATE PROCEDURE [dbo].[PR_User_Login]
    @UserName NVARCHAR(50),
    @Password NVARCHAR(50),
    @LoginSuccess BIT OUTPUT  -- Output parameter to indicate login success/failure
AS
BEGIN
    -- Initialize @LoginSuccess to 0 by default
    SET @LoginSuccess = 0;

    -- Check if a user exists with the given credentials
    IF EXISTS (
        SELECT 1
        FROM [dbo].[User]
        WHERE [dbo].[User].[UserName] = @UserName
          AND [dbo].[User].[Password] = @Password
    )
    BEGIN
        -- If user exists, select user details and set @LoginSuccess to 1
        SELECT 
            [dbo].[User].[UserID], 
            [dbo].[User].[UserName], 
            [dbo].[User].[MobileNo], 
            [dbo].[User].[Email], 
            [dbo].[User].[Password],
            [dbo].[User].[Address]
        FROM 
            [dbo].[User]
        WHERE 
            [dbo].[User].[UserName] = @UserName 
            AND [dbo].[User].[Password] = @Password;

        SET @LoginSuccess = 1;
    END
END


SELECT * FROM Bills


------------------------ Address Book
CREATE TABLE Country (
    CountryID INT PRIMARY KEY IDENTITY(1,1),
    CountryName NVARCHAR(100) NOT NULL,
    CountryCode NVARCHAR(10) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);

CREATE TABLE State (
    StateID INT PRIMARY KEY IDENTITY(1,1),
    CountryID INT NOT NULL,
    StateName NVARCHAR(100) NOT NULL,
    StateCode NVARCHAR(10),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    FOREIGN KEY (CountryID) REFERENCES Country(CountryID)
);

CREATE TABLE City (
    CityID INT PRIMARY KEY IDENTITY(1,1),
    StateID INT NOT NULL,
    CountryID INT NOT NULL,
    CityName NVARCHAR(100) NOT NULL,
    CityCode NVARCHAR(10),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    FOREIGN KEY (StateID) REFERENCES State(StateID),
    FOREIGN KEY (CountryID) REFERENCES Country(CountryID)
);

INSERT INTO Country (CountryName, CountryCode, CreatedDate) VALUES
('United States', 'US', GETDATE()),
('India', 'IN', GETDATE()),
('Australia', 'AU', GETDATE()),
('Canada', 'CA', GETDATE()),
('United Kingdom', 'UK', GETDATE()),
('Germany', 'DE', GETDATE()),
('France', 'FR', GETDATE()),
('Japan', 'JP', GETDATE()),
('China', 'CN', GETDATE()),
('Brazil', 'BR', GETDATE());

INSERT INTO State (StateName, StateCode, CountryID, CreatedDate) VALUES
('California', 'CA', 1, GETDATE()),
('Texas', 'TX', 1, GETDATE()),
('Gujarat', 'GJ', 2, GETDATE()),
('Maharashtra', 'MH', 2, GETDATE()),
('New South Wales', 'NSW', 3, GETDATE()),
('Victoria', 'VIC', 3, GETDATE()),
('Ontario', 'ON', 4, GETDATE()),
('Quebec', 'QC', 4, GETDATE()),
('England', 'ENG', 5, GETDATE()),
('Scotland', 'SCT', 5, GETDATE());

INSERT INTO City (CityName, CityCode, StateID, CountryID, CreatedDate) VALUES
('Los Angeles', 'LA', 1, 1, GETDATE()),
('Houston', 'HOU', 2, 1, GETDATE()),
('Ahmedabad', 'AMD', 3, 2, GETDATE()),
('Mumbai', 'MUM', 4, 2, GETDATE()),
('Sydney', 'SYD', 5, 3, GETDATE()),
('Melbourne', 'MEL', 6, 3, GETDATE()),
('Toronto', 'TOR', 7, 4, GETDATE()),
('Montreal', 'MTL', 8, 4, GETDATE()),
('London', 'LDN', 9, 5, GETDATE()),
('Edinburgh', 'EDI', 10, 5, GETDATE());

-- City Table Store Procedure

-- Get All Cities
CREATE PROCEDURE [dbo].[PR_LOC_City_SelectAll]
AS
BEGIN
    SELECT
        [dbo].[City].[CityID],
        [dbo].[City].[StateID],
        [dbo].[City].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[StateName],
        [dbo].[City].[CityName],
        [dbo].[City].[CityCode],
        [dbo].[City].[CreatedDate],
        [dbo].[City].[ModifiedDate]
    FROM [dbo].[City]
    LEFT OUTER JOIN [dbo].[State]
        ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
    LEFT OUTER JOIN [dbo].[Country]
        ON [dbo].[Country].[CountryID] = [dbo].[City].[CountryID];
END

-- Get City by ID
CREATE PROCEDURE [dbo].[PR_LOC_City_SelectByPK]
    @CityID INT
AS
BEGIN
    SELECT
        [dbo].[City].[CityID],
        [dbo].[City].[CityName],
        [dbo].[City].[CityCode],
        [dbo].[City].[StateID],
        [dbo].[City].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[StateName],
        [dbo].[City].[CreatedDate],
        [dbo].[City].[ModifiedDate]
    FROM [dbo].[City]
    LEFT OUTER JOIN [dbo].[State]
        ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
    LEFT OUTER JOIN [dbo].[Country]
        ON [dbo].[Country].[CountryID] = [dbo].[City].[CountryID]
    WHERE [dbo].[City].[CityID] = @CityID;
END

-- Insert City
CREATE PROCEDURE [dbo].[PR_LOC_City_Insert]
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @CountryID INT
AS
BEGIN
    INSERT INTO [dbo].[City] (
        [CityName],
        [CityCode],
        [StateID],
        [CountryID],
        [CreatedDate]
    )
    VALUES (
        @CityName,
        @CityCode,
        @StateID,
        @CountryID,
        GETDATE()
    );
END

-- Update City
CREATE PROCEDURE [dbo].[PR_LOC_City_Update]
    @CityID INT,
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @CountryID INT
AS
BEGIN
    UPDATE [dbo].[City]
    SET
        [CityName] = @CityName,
        [CityCode] = @CityCode,
        [StateID] = @StateID,
        [CountryID] = @CountryID,
        [ModifiedDate] = GETDATE()
    WHERE [CityID] = @CityID;
END

-- Delete City
CREATE PROCEDURE [dbo].[PR_LOC_City_Delete]
    @CityID INT
AS
BEGIN
    DELETE FROM [dbo].[City]
    WHERE [CityID] = @CityID;
END

-- Country Table Store Procedure

-- Get All Countries
CREATE PROCEDURE [dbo].[PR_LOC_Country_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[Country].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[Country].[CountryCode],
        [dbo].[Country].[CreatedDate],
        [dbo].[Country].[ModifiedDate]
    FROM [dbo].[Country];
END

-- Get Country by ID
CREATE PROCEDURE [dbo].[PR_LOC_Country_SelectByPK]
    @CountryID INT
AS
BEGIN
    SELECT 
        [dbo].[Country].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[Country].[CountryCode],
        [dbo].[Country].[CreatedDate],
        [dbo].[Country].[ModifiedDate]
    FROM [dbo].[Country]
    WHERE [dbo].[Country].[CountryID] = @CountryID;
END

-- Insert Country
CREATE PROCEDURE [dbo].[PR_LOC_Country_Insert]
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
    INSERT INTO [dbo].[Country] (
        [CountryName],
        [CountryCode],
        [CreatedDate]
    )
    VALUES (
        @CountryName,
        @CountryCode,
        GETDATE()
    );
END

-- Update Country
CREATE PROCEDURE [dbo].[PR_LOC_Country_Update]
    @CountryID INT,
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
    UPDATE [dbo].[Country]
    SET 
        [CountryName] = @CountryName,
        [CountryCode] = @CountryCode,
        [ModifiedDate] = GETDATE()
    WHERE 
        [CountryID] = @CountryID;
END

-- Delete Country
CREATE PROCEDURE [dbo].[PR_LOC_Country_Delete]
    @CountryID INT
AS
BEGIN
    DELETE FROM [dbo].[Country]
    WHERE [CountryID] = @CountryID;
END

-- State Table Store Procedure

-- Get All States
ALTER PROCEDURE [dbo].[PR_LOC_State_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[State].[StateID],
        [dbo].[State].[StateName],
        [dbo].[State].[StateCode],
        [dbo].[State].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[CreatedDate],
        [dbo].[State].[ModifiedDate],
        COUNT([dbo].[City].[CityID]) AS CityCount
    FROM 
        [dbo].[State]
    INNER JOIN 
        [dbo].[Country] ON [dbo].[State].[CountryID] = [dbo].[Country].[CountryID]
    LEFT JOIN 
        [dbo].[City] ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
    GROUP BY
        [dbo].[State].[StateID],
        [dbo].[State].[StateName],
        [dbo].[State].[StateCode],
        [dbo].[State].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[CreatedDate],
        [dbo].[State].[ModifiedDate]
    ORDER BY 
        [dbo].[State].[StateName];
END

-- Get State by ID
ALTER PROCEDURE [dbo].[PR_LOC_State_SelectByPK]
    @StateID INT
AS
BEGIN
    SELECT 
        [dbo].[State].[StateID],
        [dbo].[State].[StateName],
        [dbo].[State].[StateCode],
        [dbo].[State].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[CreatedDate],
        [dbo].[State].[ModifiedDate],
        COUNT([dbo].[City].[CityID]) AS CityCount
    FROM 
        [dbo].[State]
    INNER JOIN 
        [dbo].[Country] ON [dbo].[State].[CountryID] = [dbo].[Country].[CountryID]
    LEFT JOIN 
        [dbo].[City] ON [dbo].[State].[StateID] = [dbo].[City].[StateID]
    WHERE [dbo].[State].[StateID] = @StateID
    GROUP BY
        [dbo].[State].[StateID],
        [dbo].[State].[StateName],
        [dbo].[State].[StateCode],
        [dbo].[State].[CountryID],
        [dbo].[Country].[CountryName],
        [dbo].[State].[CreatedDate],
        [dbo].[State].[ModifiedDate]
    ORDER BY 
        [dbo].[State].[StateName];
END

-- Insert State
CREATE PROCEDURE [dbo].[PR_LOC_State_Insert]
    @CountryID INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10)
AS
BEGIN
    -- Insert into State
    INSERT INTO [dbo].[State] (
        CountryID,
        StateName,
        StateCode,
        CreatedDate
    )
    VALUES (
        @CountryID,
        @StateName,
        @StateCode,
        GETDATE()
    );
END

-- Update State
CREATE PROCEDURE [dbo].[PR_LOC_State_Update]
    @StateID INT,
    @CountryID INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10)
AS
BEGIN
    UPDATE [dbo].[State]
    SET 
        [dbo].[State].[CountryID] = @CountryID,
        [dbo].[State].[StateName] = @StateName,
        [dbo].[State].[StateCode] = @StateCode,
        [dbo].[State].[ModifiedDate] = GETDATE()
    WHERE 
        [dbo].[State].[StateID] = @StateID;
END

-- Delete State
CREATE PROCEDURE [dbo].[PR_LOC_State_Delete]
    @StateID INT
AS
BEGIN
    DELETE FROM [dbo].[State]
    WHERE [dbo].[State].[StateID] = @StateID;
END

-- Drop Downs

-- Get All Countries
CREATE PROCEDURE [dbo].[PR_LOC_Country_SelectComboBox]
AS 
SELECT
    [dbo].[Country].[CountryID],
    [dbo].[Country].[CountryName]
FROM [dbo].[Country]
ORDER BY [dbo].[Country].[CountryName]

-- Get States by Country ID
CREATE PROCEDURE [dbo].[PR_LOC_State_SelectComboBoxByCountryID]
@CountryID INT
AS 
SELECT
    [dbo].[State].[StateID],
    [dbo].[State].[StateName]
FROM [dbo].[State]
WHERE [dbo].[State].[CountryID] = @CountryID
ORDER BY [dbo].[State].[StateName]

-- Drop Down For State Table
CREATE PROCEDURE [dbo].[PR_State_DropDown]
AS
BEGIN
    SELECT
        [StateID],
        [StateName]
    FROM
        [dbo].[State]
	ORDER BY [StateName]
END

-- Drop Down For Country Table
CREATE PROCEDURE [dbo].[PR_Country_DropDown]
AS
BEGIN
    SELECT
        [CountryID],
        [CountryName]
    FROM
        [dbo].[Country]
	ORDER BY [CountryName]
END


INSERT INTO [DBO].[User] (UserName, Email, Password, MobileNo, Address, IsActive, CreatedAt)
VALUES
('Alice Johnson', 'alice.johnson@example.com', 'password123', '1234567890', '123 Elm Street', 1, GETDATE()),
('Bob Smith', 'bob.smith@example.com', 'password456', '2345678901', '456 Oak Avenue', 1, GETDATE()),
('Charlie Brown', 'charlie.brown@example.com', 'password789', '3456789012', '789 Pine Road', 1, GETDATE()),
('David Williams', 'david.williams@example.com', 'password101', '4567890123', '101 Maple Drive', 1, GETDATE()),
('Eve Davis', 'eve.davis@example.com', 'password102', '5678901234', '202 Birch Lane', 1, GETDATE()),
('Frank Miller', 'frank.miller@example.com', 'password103', '6789012345', '303 Cedar Blvd', 1, GETDATE()),
('Grace Lee', 'grace.lee@example.com', 'password104', '7890123456', '404 Pine Crescent', 1, GETDATE()),
('Hannah Scott', 'hannah.scott@example.com', 'password105', '8901234567', '505 Oak Grove', 1, GETDATE()),
('Ivy Carter', 'ivy.carter@example.com', 'password106', '9012345678', '606 Maple Ave', 1, GETDATE()),
('Jack Turner', 'jack.turner@example.com', 'password107', '0123456789', '707 Birch Hill', 1, GETDATE()),
('Kathy Thomas', 'kathy.thomas@example.com', 'password108', '1234567890', '808 Cedar Lane', 1, GETDATE()),
('Louis Adams', 'louis.adams@example.com', 'password109', '2345678901', '909 Oakfield St', 1, GETDATE()),
('Mandy Foster', 'mandy.foster@example.com', 'password110', '3456789012', '1010 Pine Ridge', 1, GETDATE()),
('Nancy Grant', 'nancy.grant@example.com', 'password111', '4567890123', '1111 Maple Heights', 1, GETDATE()),
('Oliver King', 'oliver.king@example.com', 'password112', '5678901234', '1212 Birch Grove', 1, GETDATE());

INSERT INTO [DBO].[Product] (ProductName, ProductPrice, ProductCode, Description, UserID, CreatedAt)
VALUES
('Laptop', 1200.00, 'LP001', 'High-performance laptop', 1, GETDATE()),
('Smartphone', 800.00, 'SP001', 'Latest model smartphone', 2, GETDATE()),
('Headphones', 150.00, 'HP001', 'Noise-canceling headphones', 3, GETDATE()),
('Keyboard', 50.00, 'KB001', 'Mechanical keyboard', 4, GETDATE()),
('Mouse', 30.00, 'MS001', 'Wireless mouse', 5, GETDATE()),
('Smartwatch', 250.00, 'SW001', 'Fitness tracking smartwatch', 6, GETDATE()),
('Tablet', 350.00, 'TB001', 'Portable tablet for reading', 7, GETDATE()),
('Charger', 20.00, 'CH001', 'Fast charging adapter', 8, GETDATE()),
('Monitor', 300.00, 'MON001', '27-inch Full HD monitor', 9, GETDATE()),
('Router', 100.00, 'RT001', 'Wi-Fi router', 10, GETDATE()),
('Printer', 200.00, 'PR001', 'Wireless printer', 11, GETDATE()),
('External HDD', 150.00, 'EH001', '1TB external hard drive', 12, GETDATE()),
('Camera', 500.00, 'CM001', 'Digital camera for photography', 13, GETDATE()),
('Microphone', 100.00, 'MIC001', 'Studio quality microphone', 14, GETDATE()),
('Speakers', 80.00, 'SPK001', 'Bluetooth portable speakers', 15, GETDATE());

INSERT INTO [DBO].[Customer] (CustomerName, HomeAddress, Email, MobileNo, GST_NO, CityName, PinCode, NetAmount, UserID, CreatedAt)
VALUES
('John Doe', '123 Main Street', 'john.doe@example.com', '9876543210', 'GST123456', 'New York', '10001', 5000.00, 1, GETDATE()),
('Jane Smith', '456 Oak Avenue', 'jane.smith@example.com', '8765432109', 'GST654321', 'Los Angeles', '90001', 3000.00, 2, GETDATE()),
('Tom Clark', '789 Pine Road', 'tom.clark@example.com', '7654321098', 'GST789123', 'Chicago', '60001', 4000.00, 3, GETDATE()),
('Lucy White', '101 Maple Drive', 'lucy.white@example.com', '6543210987', 'GST456789', 'San Francisco', '94101', 6000.00, 4, GETDATE()),
('Michael Green', '202 Birch Lane', 'michael.green@example.com', '5432109876', 'GST987654', 'Miami', '33101', 2000.00, 5, GETDATE()),
('Sophia Miller', '303 Cedar Blvd', 'sophia.miller@example.com', '4321098765', 'GST246810', 'Dallas', '75201', 3500.00, 6, GETDATE()),
('David Lee', '404 Pine Crescent', 'david.lee@example.com', '3210987654', 'GST135791', 'Houston', '77001', 4500.00, 7, GETDATE()),
('Emma Walker', '505 Oak Grove', 'emma.walker@example.com', '2109876543', 'GST112233', 'Austin', '73301', 5500.00, 8, GETDATE()),
('Lucas King', '606 Maple Ave', 'lucas.king@example.com', '1098765432', 'GST998877', 'Phoenix', '85001', 3200.00, 9, GETDATE()),
('Olivia Scott', '707 Birch Hill', 'olivia.scott@example.com', '9876543210', 'GST223344', 'Las Vegas', '89001', 2700.00, 10, GETDATE()),
('Ethan Harris', '808 Cedar Lane', 'ethan.harris@example.com', '8765432109', 'GST556677', 'Chicago', '60002', 7000.00, 11, GETDATE()),
('Mason Walker', '909 Oakfield St', 'mason.walker@example.com', '7654321098', 'GST889900', 'Denver', '80202', 4300.00, 12, GETDATE()),
('Amelia Martinez', '1010 Pine Ridge', 'amelia.martinez@example.com', '6543210987', 'GST223355', 'San Diego', '92101', 2400.00, 13, GETDATE()),
('Aiden Young', '1111 Maple Heights', 'aiden.young@example.com', '5432109876', 'GST667788', 'Seattle', '98101', 3800.00, 14, GETDATE()),
('Isabella Allen', '1212 Birch Grove', 'isabella.allen@example.com', '4321098765', 'GST445566', 'Portland', '97201', 4100.00, 15, GETDATE());

INSERT INTO [DBO].[Order] (OrderNumber, OrderDate, CustomerID, PaymentMode, TotalAmount, ShippingAddress, UserID, CreatedAt)
VALUES
('ORD001', GETDATE(), 1, 'Credit Card', 1500.00, '123 Main Street', 1, GETDATE()),
('ORD002', GETDATE(), 2, 'PayPal', 2200.00, '456 Oak Avenue', 2, GETDATE()),
('ORD003', GETDATE(), 3, 'Debit Card', 1800.00, '789 Pine Road', 3, GETDATE()),
('ORD004', GETDATE(), 4, 'Cash', 1000.00, '101 Maple Drive', 4, GETDATE()),
('ORD005', GETDATE(), 5, 'Credit Card', 2500.00, '202 Birch Lane', 5, GETDATE()),
('ORD006', GETDATE(), 6, 'Credit Card', 2800.00, '303 Cedar Blvd', 6, GETDATE()),
('ORD007', GETDATE(), 7, 'Debit Card', 2000.00, '404 Pine Crescent', 7, GETDATE()),
('ORD008', GETDATE(), 8, 'Cash', 1300.00, '505 Oak Grove', 8, GETDATE()),
('ORD009', GETDATE(), 9, 'Credit Card', 1600.00, '606 Maple Ave', 9, GETDATE()),
('ORD010', GETDATE(), 10, 'PayPal', 2100.00, '707 Birch Hill', 10, GETDATE()),
('ORD011', GETDATE(), 11, 'Debit Card', 1700.00, '808 Cedar Lane', 11, GETDATE()),
('ORD012', GETDATE(), 12, 'Cash', 1400.00, '909 Oakfield St', 12, GETDATE()),
('ORD013', GETDATE(), 13, 'Credit Card', 1900.00, '1010 Pine Ridge', 13, GETDATE()),
('ORD014', GETDATE(), 14, 'PayPal', 2400.00, '1111 Maple Heights', 14, GETDATE()),
('ORD015', GETDATE(), 15, 'Debit Card', 2200.00, '1212 Birch Grove', 15, GETDATE());

INSERT INTO [DBO].[OrderDetail] (OrderID, ProductID, Quantity, Amount, TotalAmount, UserID, CreatedAt)
VALUES
(1, 1, 1, 1200.00, 1200.00, 1, GETDATE()),
(1, 2, 1, 800.00, 800.00, 1, GETDATE()),
(2, 3, 2, 150.00, 300.00, 2, GETDATE()),
(2, 4, 1, 50.00, 50.00, 2, GETDATE()),
(3, 5, 1, 30.00, 30.00, 3, GETDATE()),
(3, 6, 2, 250.00, 500.00, 3, GETDATE()),
(4, 7, 1, 350.00, 350.00, 4, GETDATE()),
(4, 8, 1, 20.00, 20.00, 4, GETDATE()),
(5, 9, 1, 300.00, 300.00, 5, GETDATE()),
(5, 10, 2, 100.00, 200.00, 5, GETDATE()),
(6, 11, 1, 200.00, 200.00, 6, GETDATE()),
(6, 12, 1, 150.00, 150.00, 6, GETDATE()),
(7, 13, 1, 500.00, 500.00, 7, GETDATE()),
(7, 14, 2, 100.00, 200.00, 7, GETDATE()),
(8, 15, 3, 80.00, 240.00, 8, GETDATE());

INSERT INTO [DBO].[Bills] (BillNumber, BillDate, OrderID, TotalAmount, Discount, NetAmount, UserID, CreatedAt)
VALUES
('BILL001', GETDATE(), 1, 1500.00, 0.00, 1500.00, 1, GETDATE()),
('BILL002', GETDATE(), 2, 2200.00, 100.00, 2100.00, 2, GETDATE()),
('BILL003', GETDATE(), 3, 1800.00, 0.00, 1800.00, 3, GETDATE()),
('BILL004', GETDATE(), 4, 1000.00, 50.00, 950.00, 4, GETDATE()),
('BILL005', GETDATE(), 5, 2500.00, 150.00, 2350.00, 5, GETDATE()),
('BILL006', GETDATE(), 6, 2800.00, 200.00, 2600.00, 6, GETDATE()),
('BILL007', GETDATE(), 7, 2000.00, 100.00, 1900.00, 7, GETDATE()),
('BILL008', GETDATE(), 8, 1300.00, 50.00, 1250.00, 8, GETDATE()),
('BILL009', GETDATE(), 9, 1600.00, 0.00, 1600.00, 9, GETDATE()),
('BILL010', GETDATE(), 10, 2100.00, 100.00, 2000.00, 10, GETDATE()),
('BILL011', GETDATE(), 11, 1700.00, 0.00, 1700.00, 11, GETDATE()),
('BILL012', GETDATE(), 12, 1400.00, 50.00, 1350.00, 12, GETDATE()),
('BILL013', GETDATE(), 13, 1900.00, 0.00, 1900.00, 13, GETDATE()),
('BILL014', GETDATE(), 14, 2400.00, 150.00, 2250.00, 14, GETDATE()),
('BILL015', GETDATE(), 15, 2200.00, 100.00, 2100.00, 15, GETDATE());

SELECT * FROM [User]
SELECT * FROM Product
SELECT * FROM Customer
SELECT * FROM [Order]
SELECT * FROM OrderDetail
SELECT * FROM Bills

 -- This is Valid But Not Use Mostly
ALTER PROCEDURE [dbo].[PR_LOC_Customer_CountCustomer]
AS
BEGIN
    SELECT COUNT(*) AS CustomerCount
    FROM [DBO].[Customer]
END

CREATE PROCEDURE [dbo].[PR_LOC_Product_CountProduct]
AS
BEGIN
    SELECT COUNT(*) AS ProductCount
    FROM [DBO].[Product]
END

GO
CREATE PROCEDURE [dbo].[PR_LOC_Order_CountOrders]
AS
BEGIN
    SELECT COUNT(*) AS OrderCount
    FROM [DBO].[Orders]
END

-- This is Use IN Most Of Dashboard
GO
ALTER PROCEDURE [dbo].[usp_GetDashboardData]
AS
BEGIN
    -- Enable NOCOUNT for better performance
    SET NOCOUNT ON;
-- SET NOCOUNT ON: Suppresses the message from being returned. This prevents the sending of DONEINPROC messages to the client for each
-- statement in a stored procedure.
-- SET NOCOUNT OFF: Includes the message in the result set. 
    -- Temporary tables for organized data fetching
	CREATE TABLE #Counts (
        Metric NVARCHAR(255),
        Value INT
		);

    CREATE TABLE #RecentOrders (
        OrderID INT,
		OrderNumber VARCHAR(50),
        CustomerName NVARCHAR(255),
        OrderDate DATETIME,
        Status NVARCHAR(50)
    );

    CREATE TABLE #RecentProducts (
        ProductID INT,
        ProductName NVARCHAR(255),
        Category NVARCHAR(255),
        AddedDate DATETIME,
        StockQuantity INT
    );

    CREATE TABLE #TopCustomers (
        CustomerName NVARCHAR(255),
        TotalOrders INT,
        Email NVARCHAR(255)
    );

    CREATE TABLE #TopSellingProducts (
        ProductName NVARCHAR(255),
        TotalSoldQuantity INT,
        Category NVARCHAR(255)
    );

    ---- Step 1: Get Counts
    --
	INSERT INTO #Counts
        SELECT 'TotalCustomers', COUNT(*) FROM Customer
    INSERT INTO #Counts
	    SELECT 'TotalProducts', COUNT(*) FROM Product
	INSERT INTO #Counts
		SELECT 'TotalOrders',COUNT(*) FROM [Order]
	INSERT INTO #Counts
		SELECT 'TotalBills',COUNT(*) FROM Bills;
		
    --    (SELECT COUNT(*) FROM Customer) AS TotalCustomers,
    --    (SELECT COUNT(*) FROM Product) AS TotalProducts,
    --    (SELECT COUNT(*) FROM [Order]) AS TotalOrders,
    --    (SELECT COUNT(*) FROM Bills) AS TotalBills;

    -- Step 2: Get Recent 10 Orders
    INSERT INTO #RecentOrders
    SELECT TOP 10
        O.OrderID,
		o.OrderNumber,
        C.CustomerName,
        O.OrderDate,
        O.[Status]
    FROM [Order] O
    INNER JOIN Customer C ON O.CustomerID = C.CustomerID
    ORDER BY O.OrderDate DESC;

    -- Step 3: Get Recent 10 Newly Added Products
    INSERT INTO #RecentProducts
    SELECT TOP 10
        ProductID,
        ProductName,
        Category,
        CreatedAt,
        StockQuantity
    FROM Product
    ORDER BY CreatedAt DESC;

    -- Step 4: Get Top 10 Customers by Order Count
    INSERT INTO #TopCustomers
    SELECT TOP 10
        C.CustomerName,
        COUNT(O.OrderID) AS TotalOrders,
        C.Email
    FROM [Order] O
    INNER JOIN Customer C ON O.CustomerID = C.CustomerID
    GROUP BY C.CustomerName, C.Email
    ORDER BY COUNT(O.OrderID) DESC;

    -- Step 5: Get Top 10 Selling Products
    INSERT INTO #TopSellingProducts
    SELECT TOP 10
        P.ProductName,
        SUM(OI.Quantity) AS TotalSoldQuantity,
        P.Category
    FROM OrderDetail OI
    INNER JOIN Product P ON OI.ProductID = P.ProductID
    GROUP BY P.ProductName, P.Category
    ORDER BY SUM(OI.Quantity) DESC;

    -- Output Results
    -- Output Counts
    SELECT * FROM #Counts;

    -- Output Recent Orders
    SELECT * FROM #RecentOrders;

    -- Output Recent Products
    SELECT * FROM #RecentProducts;

    -- Output Top Customers
    SELECT * FROM #TopCustomers;

    -- Output Top Selling Products
    SELECT * FROM #TopSellingProducts;

    -- Cleanup Temporary Tables
    DROP TABLE #RecentOrders;
    DROP TABLE #RecentProducts;
    DROP TABLE #TopCustomers;
    DROP TABLE #TopSellingProducts;
END;

SELECT * FROM [Order]