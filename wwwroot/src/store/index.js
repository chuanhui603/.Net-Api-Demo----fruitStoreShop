import { createStore } from 'vuex'
import auth from './modules/auth'
import cart from './modules/cart'
import products from './modules/products'
import categories from './modules/categories'
import orders from './modules/orders'
import wishlist from './modules/wishlist'

export default createStore({
  modules: {
    auth,
    cart,
    products,
    categories,
    orders,
    wishlist
  }
}) 