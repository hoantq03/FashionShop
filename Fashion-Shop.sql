create database Shop
go

--use master
--go
--drop database Shop
--go

use Shop
go

-- carts table
CREATE TABLE carts (
    id varchar(255) NOT NULL PRIMARY KEY,
    total_amount decimal NOT NULL DEFAULT 0,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1'
);
GO

-- users table
CREATE TABLE users (
    id varchar(255) NOT NULL PRIMARY KEY,
    first_name varchar(100) NOT NULL,
    last_name varchar(100) NOT NULL,
    email varchar(100) NOT NULL UNIQUE,
    role varchar(100) NOT NULL DEFAULT 'customer',
    password varchar(255) NOT NULL,
    phone varchar(20) NOT NULL,
    birthday datetime2 NOT NULL,
    status smallint DEFAULT 1,
    cart_id varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_users_carts FOREIGN KEY (cart_id) REFERENCES carts(id)
);
GO

-- addresses table
CREATE TABLE addresses (
    id varchar(255) NOT NULL PRIMARY KEY,
    address_line varchar(255) NOT NULL,
    district varchar(255) NOT NULL,
    ward varchar(255) NOT NULL,
    city varchar(255) NOT NULL,
    postal_code varchar(255) NOT NULL,
    country varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',
    user_id varchar(255) NOT NULL,

    CONSTRAINT FK_addresses_users FOREIGN KEY (user_id) REFERENCES users(id)
);
GO

-- products table
CREATE TABLE products (
    id varchar(255) NOT NULL PRIMARY KEY,
    name varchar(100) NOT NULL,
    imageUrl varchar(100) NOT NULL,
    price decimal NOT NULL DEFAULT 0,
    description varchar(255) NOT NULL,
    type varchar(255) NOT NULL,
    user_id varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_products_users FOREIGN KEY (user_id) REFERENCES users(id)
);
GO

-- cart_details table
CREATE TABLE cart_details (
    id varchar(255) NOT NULL PRIMARY KEY,
    cart_id varchar(255) NOT NULL,
    product_id varchar(255) NOT NULL,
    quantity int NOT NULL DEFAULT 0,
    total_amount decimal NOT NULL DEFAULT 0,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_cart_details_carts FOREIGN KEY (cart_id) REFERENCES carts(id),
    CONSTRAINT FK_cart_details_products FOREIGN KEY (product_id) REFERENCES products(id)
);
GO

-- discounts table
CREATE TABLE discounts (
    id varchar(255) NOT NULL PRIMARY KEY,
    name varchar(255) NOT NULL,
    description varchar(255) NOT NULL,
    type varchar(255) NOT NULL DEFAULT 'fixed_number',
    value int NOT NULL,
    code varchar(255) NOT NULL,
    start_date datetime2 NOT NULL DEFAULT GETDATE(),
    end_date datetime2 NOT NULL DEFAULT GETDATE() + '7 days',
    number_of_use int NOT NULL,
    min_order_value decimal NOT NULL,
    status varchar(100) NOT NULL,
    user_id varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_discounts_users FOREIGN KEY (user_id) REFERENCES users(id)
);
GO


-- shippers table
CREATE TABLE shippers (
    id varchar(255) NOT NULL PRIMARY KEY,
    company_name varchar(100) NOT NULL,
    phone int NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1'
);
GO

-- orders table
CREATE TABLE orders (
    id varchar(255) NOT NULL PRIMARY KEY,
    total_amount decimal NOT NULL DEFAULT 0,
    order_date datetime2 NOT NULL DEFAULT GETDATE(),
    bill_date datetime2 NOT NULL DEFAULT GETDATE(),
    status varchar(100) NOT NULL,
    ship_cost decimal NOT NULL DEFAULT 0,
    shipper_id varchar(255),
    user_id varchar(255) NOT NULL,
    discount_id varchar(255),
    address_id varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_orders_users FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT FK_orders_shippers FOREIGN KEY (shipper_id) REFERENCES shippers(id),
    CONSTRAINT FK_orders_discounts FOREIGN KEY (discount_id) REFERENCES discounts(id),
    CONSTRAINT FK_orders_addresses FOREIGN KEY (address_id) REFERENCES addresses(id)
);
GO

-- discount_used_detail table
CREATE TABLE discount_used_detail (
    id varchar(255) NOT NULL PRIMARY KEY,
    user_id varchar(255) NOT NULL,
    discount_id varchar(255) NOT NULL,
    order_id varchar(255) NOT NULL,
    used_date datetime2 NOT NULL DEFAULT GETDATE(),

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_discount_used_detail_users FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT FK_discount_used_detail_discounts FOREIGN KEY (discount_id) REFERENCES discounts(id),
    CONSTRAINT FK_discount_used_detail_orders FOREIGN KEY (order_id) REFERENCES orders(id)
);
GO

-- inventories table
CREATE TABLE inventories (
    id varchar(255) NOT NULL PRIMARY KEY,
    product_id varchar(255),
    user_id varchar(255) NOT NULL,
    stock int NOT NULL,
    location varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_inventories_products FOREIGN KEY (product_id) REFERENCES products(id),
    CONSTRAINT FK_inventories_users FOREIGN KEY (user_id) REFERENCES users(id)
);
GO

-- order_details table
CREATE TABLE order_details (
    id varchar(255) NOT NULL PRIMARY KEY,
    quantity int NOT NULL DEFAULT 0,
    total_amount decimal NOT NULL DEFAULT 0,
    order_id varchar(255) NOT NULL,
    product_id varchar(255) NOT NULL,

    created_at datetime2 NOT NULL DEFAULT GETDATE(),
    created_by varchar(255) NOT NULL DEFAULT '1',
    updated_at datetime2 NOT NULL DEFAULT GETDATE(),
    updated_by varchar(255) NOT NULL DEFAULT '1',

    CONSTRAINT FK_order_details_products FOREIGN KEY (product_id) REFERENCES products(id),
    CONSTRAINT FK_order_details_orders FOREIGN KEY (order_id) REFERENCES orders(id)
);
GO

