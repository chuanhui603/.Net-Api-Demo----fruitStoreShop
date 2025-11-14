import axios from 'axios'

const state = {
  items: []
}

const mutations = {
  SET_WISHLIST(state, items) {
    state.items = items
    localStorage.setItem('wishlist', JSON.stringify(items))
  },
  ADD_TO_WISHLIST(state, item) {
    if (!state.items.some(i => i.id === item.id)) {
      state.items.push(item)
      localStorage.setItem('wishlist', JSON.stringify(state.items))
    }
  },
  REMOVE_FROM_WISHLIST(state, id) {
    state.items = state.items.filter(item => item.id !== id)
    localStorage.setItem('wishlist', JSON.stringify(state.items))
  },
  CLEAR_WISHLIST(state) {
    state.items = []
    localStorage.removeItem('wishlist')
  }
}

const actions = {
  async fetchWishlist({ commit }) {
    try {
      const response = await axios.get('/Wishlist')
      commit('SET_WISHLIST', response.data)
    } catch (error) {
      throw error
    }
  },
  async addToWishlist({ commit }, productId) {
    try {
      const response = await axios.post('/Wishlist', { productId })
      commit('ADD_TO_WISHLIST', response.data)
      return response.data
    } catch (error) {
      throw error
    }
  },
  async removeFromWishlist({ commit }, id) {
    try {
      await axios.delete(`/Wishlist/${id}`)
      commit('REMOVE_FROM_WISHLIST', id)
    } catch (error) {
      throw error
    }
  },
  async clearWishlist({ commit }) {
    try {
      await axios.delete('/Wishlist')
      commit('CLEAR_WISHLIST')
    } catch (error) {
      throw error
    }
  }
}

const getters = {
  wishlistItems: state => state.items,
  wishlistItemCount: state => state.items.length,
  isInWishlist: state => id => state.items.some(item => item.id === id)
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 