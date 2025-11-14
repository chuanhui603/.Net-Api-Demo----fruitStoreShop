import axios from 'axios'

const state = {
  items: JSON.parse(localStorage.getItem('cart')) || []
}

const mutations = {
  SET_CART_ITEMS(state, items) {
    state.items = items
    localStorage.setItem('cart', JSON.stringify(items))
  },
  ADD_TO_CART(state, item) {
    const existingItem = state.items.find(i => i.id === item.id)
    if (existingItem) {
      existingItem.quantity += item.quantity
    } else {
      state.items.push(item)
    }
    localStorage.setItem('cart', JSON.stringify(state.items))
  },
  UPDATE_QUANTITY(state, { id, quantity }) {
    const item = state.items.find(i => i.id === id)
    if (item) {
      item.quantity = quantity
      localStorage.setItem('cart', JSON.stringify(state.items))
    }
  },
  REMOVE_FROM_CART(state, id) {
    state.items = state.items.filter(item => item.id !== id)
    localStorage.setItem('cart', JSON.stringify(state.items))
  },
  CLEAR_CART(state) {
    state.items = []
    localStorage.removeItem('cart')
  }
}

const actions = {
  async addToCart({ commit }, product) {
    try {
      const existingItem = state.items.find(i => i.id === product.id)
      if (existingItem) {
        commit('UPDATE_QUANTITY', { id: product.id, quantity: existingItem.quantity + 1 })
      } else {
        commit('ADD_TO_CART', { ...product, quantity: 1 })
      }
    } catch (error) {
      throw error
    }
  },
  async updateQuantity({ commit }, { id, quantity }) {
    try {
      commit('UPDATE_QUANTITY', { id, quantity })
    } catch (error) {
      throw error
    }
  },
  async removeFromCart({ commit }, id) {
    try {
      commit('REMOVE_FROM_CART', id)
    } catch (error) {
      throw error
    }
  },
  async clearCart({ commit }) {
    commit('CLEAR_CART')
  }
}

const getters = {
  cartItems: state => state.items,
  cartTotal: state => state.items.reduce((total, item) => total + (item.price * item.quantity), 0),
  cartItemCount: state => state.items.reduce((count, item) => count + item.quantity, 0)
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 