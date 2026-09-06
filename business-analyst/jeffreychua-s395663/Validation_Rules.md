# Validation Rules for eShopOnWeb User Stories

## US-001: Browse Product Catalog

| Field | Validation Rule | Error Message | Severity |
|-------|----------------|---------------|----------|
| Search input | Max 100 characters | "Search term must be less than 100 characters" | Warning |
| Category filter | Must exist in database | "Invalid category selected" | Error |
| Page number | Must be positive integer, default 1 | "Invalid page number" | Error |
| Page size | Must be 10, 20, or 50 | "Invalid page size. Choose 10, 20, or 50" | Error |

---

## US-002: Add Items to Shopping Cart

| Field | Validation Rule | Error Message | Severity |
|-------|----------------|---------------|----------|
| Product ID | Must exist and product status = Active | "Product not available or has been removed" | Error |
| Quantity | Integer, minimum 1, maximum 99 | "Quantity must be between 1 and 99" | Error |
| Stock check | Quantity <= UnitsInStock | "Only {stock} items available in stock" | Error |
| Duplicate item | If same product already in cart, merge quantities | (Auto-merge, no error) | Info |
| Cart limit | Max 50 unique items per cart | "Cart is full. Please remove items before adding more" | Error |

---

## US-003: User Registration & Login

| Field | Validation Rule | Error Message | Severity |
|-------|----------------|---------------|----------|
| Email | Valid RFC 5322 email format | "Please enter a valid email address" | Error |
| Email uniqueness | Must not exist in database | "This email is already registered. Please log in" | Error |
| Password | Min 6 chars, 1 uppercase, 1 lowercase, 1 number | "Password must be at least 6 characters with 1 uppercase, 1 lowercase, and 1 number" | Error |
| Confirm password | Must exactly match password field | "Passwords do not match" | Error |
| First name | 2-50 characters, alphabetic only | "First name must be 2-50 alphabetic characters" | Error |
| Last name | 2-50 characters, alphabetic only | "Last name must be 2-50 alphabetic characters" | Error |
| Login email | Must exist in database | "Invalid email or password" | Error (generic) |
| Login password | Must match hashed password | "Invalid email or password" | Error (generic) |
| Account lockout | After 5 failed attempts, lock 15 minutes | "Account temporarily locked. Try again in 15 minutes" | Error |

---

## US-004: Place Order & Checkout

| Field | Validation Rule | Error Message | Severity |
|-------|----------------|---------------|----------|
| Cart total | Must be greater than $0 | "Your cart is empty. Add items before checkout" | Error |
| Shipping address | Min 10 characters, required | "Valid shipping address is required" | Error |
| Full name | 2-100 characters, required | "Full name is required (2-100 characters)" | Error |
| Street address | 5-200 characters, required | "Street address is required (5-200 characters)" | Error |
| City | 2-50 characters, alphabetic only | "City must be 2-50 alphabetic characters" | Error |
| ZIP code | 4-10 alphanumeric characters | "ZIP code must be 4-10 alphanumeric characters" | Error |
| Country | Must be from predefined list | "Please select a valid country" | Error |
| Order items | All products must have sufficient stock | "Some items in your cart are out of stock" | Error |
| Order items | Product prices must match current catalog | "Product prices have changed. Please review your cart" | Warning |
| Session timeout | Checkout must complete within 30 minutes | "Session expired. Please restart checkout" | Error |

---

## US-005: Admin Manage Product Catalog

| Field | Validation Rule | Error Message | Severity |
|-------|----------------|---------------|----------|
| Product name | 3-100 characters, required, unique | "Product name must be 3-100 characters and unique" | Error |
| Price | Must be > 0, max 2 decimal places | "Price must be greater than 0 with max 2 decimal places" | Error |
| Category | Must select existing category from dropdown | "Please select a valid category" | Error |
| Description | 10-2000 characters | "Description must be 10-2000 characters" | Error |
| Image URL | Valid URL format (http/https) | "Please enter a valid image URL" | Error |
| Units in stock | Integer, 0-9999 | "Stock must be between 0 and 9999" | Error |
| Product status | Must be Active, Inactive, or Discontinued | "Invalid product status" | Error |
| Admin role | Current user must have Admin role | "Access denied. Admin privileges required" | Error |
| Delete confirmation | Must type product name to confirm deletion | "Please type the product name to confirm deletion" | Error |

---

## Cross-Cutting Validation Rules

| Rule | Applies To | Description |
|------|-----------|-------------|
| XSS Prevention | All text inputs | Sanitize HTML tags, reject `<script>` tags |
| SQL Injection | All search/filter inputs | Use parameterized queries only |
| Rate Limiting | Login, Registration | Max 10 requests per minute per IP |
| CSRF Token | All POST/PUT/DELETE requests | Valid CSRF token required |
| Input Length | All free-text fields | Max 5000 characters unless specified otherwise |
