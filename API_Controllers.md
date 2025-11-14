水水水果電商網站 API 與前端畫面規劃
一、後端 API 功能總覽
1. 認證 AuthController
POST   /api/Auth/login：用戶登入（帳號、密碼 → JWT Token）
GET    /api/Auth/logout：用戶登出（需授權）
POST   /api/Auth/register：用戶註冊（帳號、密碼、基本資訊）
2. 會員管理 MemberController
GET    /api/Member：所有會員資料
GET    /api/Member/{id}：單一會員資料
POST   /api/Member：新增會員
PUT    /api/Member/{id}：更新會員
DELETE /api/Member/{id}：刪除會員
3. 商品管理 ProductController
GET    /api/Product：所有商品
GET    /api/Product/{id}：單一商品
POST   /api/Product/Create：新增商品
PUT    /api/Product/{id}：更新商品
DELETE /api/Product/Delete：刪除商品（傳商品ID）
GET    /api/Product/search：搜尋商品（關鍵字、類別、價格）
GET    /api/Product/category/{categoryId}：指定類別商品
4. 類別管理 CategoryController
GET    /api/Category：所有類別
GET    /api/Category/{id}：單一類別
POST   /api/Category：新增類別
PUT    /api/Category/{id}：更新類別
DELETE /api/Category/{id}：刪除類別
5. 購物車管理 CartController
GET    /api/Cart：取得購物車內容
POST   /api/Cart/add：加入商品
PUT    /api/Cart/update：更新數量
DELETE /api/Cart/remove/{id}：移除商品
6. 訂單管理 OrdersController
GET    /api/Orders：所有訂單
GET    /api/Orders/page={page}/top={pageSize}：分頁查詢
GET    /api/Orders/{id}：單一訂單
POST   /api/Orders：新增訂單
PUT    /api/Orders/{id}：更新訂單
DELETE /api/Orders/{id}：刪除訂單
GET    /api/Orders/status/{id}：查詢狀態
PUT    /api/Orders/status/{id}：更新狀態（需授權）
7. 優惠券管理 CouponController
GET    /api/Coupon：所有優惠券
GET    /api/Coupon/{id}：單一優惠券
GET    /api/Coupon/page={page}/top={pageSize}：分頁查詢
POST   /api/Coupon/create：新增
POST   /api/Coupon/register：註冊優惠券代碼
PUT    /api/Coupon/update：更新
DELETE /api/Coupon/{id}：刪除
8. 庫存管理 InventoryController
GET    /api/Inventory/{productId}：查詢庫存
PUT    /api/Inventory/update：更新庫存
9. 收件人管理 RecipientController
GET    /api/Recipient：所有收件人
GET    /api/Recipient/{id}：單一收件人
POST   /api/Recipient：新增
PUT    /api/Recipient/{id}：更新
DELETE /api/Recipient/{id}：刪除
10. 付款 PayMentController
POST   /api/PayMent/LinePay/Create：建立LinePay請求
POST   /api/PayMent/LinePay/Confirm：確認LinePay
GET    /api/PayMent/LinePay/Cancel：取消LinePay
11. 評價 ReviewController
GET    /api/Review/product/{productId}：商品評價清單
POST   /api/Review：提交評價
PUT    /api/Review/{id}：更新評價（需授權）
DELETE /api/Review/{id}：刪除評價（需授權）
12. 願望清單 WishlistController
GET    /api/Wishlist：用戶願望清單
POST   /api/Wishlist/add：加入願望清單
DELETE /api/Wishlist/remove/{id}：移除商品
13. 通知 NotificationController
GET    /api/Notification：用戶通知清單
POST   /api/Notification/send：發送通知（需授權）
二、前端畫面規劃
1. 首頁
熱門水果、促銷活動、推薦商品
輪播圖、商品推薦卡、搜尋列、類別下拉、導航欄
2. 商品列表頁
商品清單、篩選（類別/價格）、排序、分頁
商品卡片、篩選面板、分頁導航
3. 商品詳情頁
商品資訊、加入購物車/願望清單、評價區
商品圖片、資訊、數量選擇、按鈕、評價
4. 購物車頁
商品清單、數量修改、移除、總金額、優惠券、結帳
5. 結帳頁
訂單摘要、收件人選擇、付款方式、確認訂單
6. 訂單追蹤頁
歷史訂單、狀態、詳情
7. 會員中心
個人資料、訂單、願望清單、收件人管理
8. 登入/註冊頁
登入表單、註冊表單
9. 評價頁
評價表單、評價清單