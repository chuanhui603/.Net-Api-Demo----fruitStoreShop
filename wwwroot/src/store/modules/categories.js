import axios from 'axios'

const state = {
  list: [],
  current: null,
  total: 0
}

const mutations = {
  SET_CATEGORIES(state, { categories, total }) {
    state.list = categories
    state.total = total
  },
  SET_CURRENT_CATEGORY(state, category) {
    state.current = category
  }
}

const actions = {
  async fetchCategories({ commit }, params = {}) {
    try {
      const response = await axios.get('/Categories', { params })
      commit('SET_CATEGORIES', {
        categories: response.data || [],
        total: response.data?.length || 0
      })
    } catch (error) {
      console.error('Error fetching categories:', error)
      commit('SET_CATEGORIES', {
        categories: [],
        total: 0
      })
    }
  },
  async fetchCategory({ commit }, id) {
    try {
      const response = await axios.get(`/Categories/${id}`)
      commit('SET_CURRENT_CATEGORY', response.data)
    } catch (error) {
      throw error
    }
  }
}

const getters = {
  categories: state => state.list,
  currentCategory: state => state.current,
  totalCategories: state => state.total
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 