// API 基礎配置
export const API_CONFIG = {
  BASE_URL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
  TIMEOUT: 10000,
  ENDPOINTS: {
    AUTH: {
      LOGIN: '/auth/login',
      REGISTER: '/auth/register',
      LOGOUT: '/auth/logout',
      REFRESH: '/auth/refresh'
    },
    PRODUCTS: {
      LIST: '/products',
      DETAIL: '/products',
      SEARCH: '/products/search',
      CATEGORIES: '/products/categories'
    },
    CART: {
      GET: '/cart',
      ADD: '/cart/add',
      UPDATE: '/cart/update',
      REMOVE: '/cart/remove',
      CLEAR: '/cart/clear'
    },
    ORDERS: {
      CREATE: '/orders',
      LIST: '/orders',
      DETAIL: '/orders'
    },
    USER: {
      PROFILE: '/user/profile',
      UPDATE: '/user/update'
    }
  }
}

// 建立完整的 API URL
export const createApiUrl = (endpoint) => {
  return `${API_CONFIG.BASE_URL}${endpoint}`
}

// 預設的 API 端點
export default {
  // 認證相關
  login: () => createApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGIN),
  register: () => createApiUrl(API_CONFIG.ENDPOINTS.AUTH.REGISTER),
  logout: () => createApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGOUT),
  
  // 產品相關
  products: () => createApiUrl(API_CONFIG.ENDPOINTS.PRODUCTS.LIST),
  productDetail: (id) => createApiUrl(`${API_CONFIG.ENDPOINTS.PRODUCTS.DETAIL}/${id}`),
  productSearch: () => createApiUrl(API_CONFIG.ENDPOINTS.PRODUCTS.SEARCH),
  categories: () => createApiUrl(API_CONFIG.ENDPOINTS.PRODUCTS.CATEGORIES),
  
  // 購物車相關
  cart: () => createApiUrl(API_CONFIG.ENDPOINTS.CART.GET),
  addToCart: () => createApiUrl(API_CONFIG.ENDPOINTS.CART.ADD),
  updateCart: () => createApiUrl(API_CONFIG.ENDPOINTS.CART.UPDATE),
  removeFromCart: () => createApiUrl(API_CONFIG.ENDPOINTS.CART.REMOVE),
  
  // 訂單相關
  orders: () => createApiUrl(API_CONFIG.ENDPOINTS.ORDERS.LIST),
  createOrder: () => createApiUrl(API_CONFIG.ENDPOINTS.ORDERS.CREATE),
  orderDetail: (id) => createApiUrl(`${API_CONFIG.ENDPOINTS.ORDERS.DETAIL}/${id}`)
}