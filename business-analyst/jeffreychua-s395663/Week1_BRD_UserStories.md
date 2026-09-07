# Week 1 — Requirements & Domain Foundations

## Business Requirements Document (BRD)

**Project:** eShopOnWeb E-Commerce Platform
**Analyst:** Jeffrey Chua Jia Hao
**Date:** September 2026
**Version:** 1.0

---

### 1. Project Overview

eShopOnWeb is a reference e-commerce web application built on ASP.NET Core by Microsoft. It demonstrates modern software engineering practices including Clean Architecture, Domain-Driven Design (DDD), and Entity Framework Core. Our team will use this open-source project to practice and demonstrate software engineering processes across all roles.

### 2. Business Objectives

| # | Objective | Priority |
| --- | --- | --- |
| 1 | Enable customers to browse and purchase products online | High |
| 2 | Provide secure user authentication and account management | High |
| 3 | Allow administrators to manage product catalog and orders | High |
| 4 | Support shopping cart persistence across sessions | Medium |
| 5 | Deliver a responsive and user-friendly web interface | Medium |

### 3. Key Stakeholders

| Stakeholder | Role | Interest / Concern |
| --- | --- | --- |
| Customer | End user | Easy browsing, smooth checkout, order tracking |
| Admin | System manager | Catalog management, order oversight, reporting |
| Developer | Technical team | Clean code, extensible architecture, documentation |
| Tester | QA team | Testable features, clear acceptance criteria |
| Business Owner | Sponsor | Revenue, user satisfaction, system reliability |

### 4. Scope — In Scope

- Product catalog browsing with filtering
- Shopping cart operations (add, remove, update quantity)
- User registration, login, and profile management
- Order placement and checkout flow
- Admin dashboard for catalog and order management
- Order history for logged-in users

### 5. Scope — Out of Scope

- Real payment gateway integration (mock/demo only)
- Shipping carrier integration and tracking
- Mobile native application (web-responsive only)
- Multi-vendor marketplace features
- Advanced analytics and BI dashboards

### 6. Success Criteria

- All 5 core user stories implemented with passing acceptance criteria
- End-to-end purchase flow demonstrated successfully
- Code review completed with no critical issues
- Test coverage meets minimum threshold (80%)
- Team can present complete workflow to facilitator

---

## User Stories & Acceptance Criteria

**Feature Focus:** "Complete Purchase Flow" — from browsing to order confirmation

---

### US-001: Browse Product Catalog

**As a** customer,
**I want to** browse products by category and view details,
**So that** I can find items I want to purchase.

**Acceptance Criteria:**

- [ ] Customer can view a paginated list of products (10 per page)
- [ ] Customer can filter products by category (e.g., Electronics, Clothing)
- [ ] Each product displays: name, price, description, and image
- [ ] Customer can click into a product detail page
- [ ] Search functionality returns relevant results by product name

---

### US-002: Add Items to Shopping Cart

**As a** customer,
**I want to** add products to my shopping cart,
**So that** I can collect multiple items before checking out.

**Acceptance Criteria:**

- [ ] Customer can add an item to cart from product detail page
- [ ] Cart icon in header updates with total item count
- [ ] Customer can view cart contents with item names, quantities, and subtotals
- [ ] Customer can remove an item from cart
- [ ] Customer can update item quantity in cart (1–99)
- [ ] Cart total price updates automatically when quantity changes
- [ ] Cart persists for logged-in users across sessions

---

### US-003: User Registration & Login

**As a** new visitor,
**I want to** create an account and log in,
**So that** I can save my cart and view my order history.

**Acceptance Criteria:**

- [ ] User can register with email, password, and confirm password
- [ ] Password must be minimum 6 characters with at least 1 uppercase and 1 number
- [ ] Email must be unique (duplicate emails rejected with clear error)
- [ ] Confirmation message displayed after successful registration
- [ ] User is automatically logged in after registration
- [ ] User can log in with email and password
- [ ] Failed login shows generic error (security best practice)
- [ ] User can log out and session is cleared

---

### US-004: Place Order & Checkout

**As a** logged-in customer,
**I want to** complete my purchase with shipping details,
**So that** I can receive the products I ordered.

**Acceptance Criteria:**

- [ ] Customer can proceed from cart to checkout page
- [ ] Checkout page displays order summary (items, quantities, total)
- [ ] Customer can enter and save shipping address
- [ ] Customer can review order before final confirmation
- [ ] Order is saved and assigned a unique order number
- [ ] Order appears in customer's order history
- [ ] Cart is cleared after successful order placement
- [ ] Confirmation message shown with order number

---

### US-005: Admin Manage Product Catalog

**As an** administrator,
**I want to** add, edit, and delete products,
**So that** the store catalog remains accurate and up to date.

**Acceptance Criteria:**

- [ ] Admin can access admin dashboard via authenticated route
- [ ] Admin can add new product with: name, description, price, category, image URL
- [ ] Admin can edit existing product details
- [ ] Admin can delete a product with confirmation prompt
- [ ] Admin can view list of all products with search/filter
- [ ] Changes reflect immediately on the public catalog
- [ ] Only users with Admin role can access catalog management