import axios from 'axios'

const state = {
  orders: [],
  currentOrder: null
}

const mutations = {
  SET_ORDERS(state, orders) {
    state.orders = orders
  },
  SET_CURRENT_ORDER(state, order) {
    state.currentOrder = order
  },
  ADD_ORDER(state, order) {
    state.orders.unshift(order)
  },
  UPDATE_ORDER_STATUS(state, { id, status }) {
    const order = state.orders.find(o => o.id === id)
    if (order) {
      order.status = status
    }
  }
}

const actions = {
  async fetchOrders({ commit }) {
    try {
      const response = await axios.get('/Orders')
      commit('SET_ORDERS', response.data)
    } catch (error) {
      throw error
    }
  },
  async fetchOrder({ commit }, orderId) {
    try {
      const response = await axios.get(`/Orders/${orderId}`)
      commit('SET_CURRENT_ORDER', response.data)
    } catch (error) {
      throw error
    }
  },
  async createOrder({ commit }, orderData) {
    try {
      const response = await axios.post('/Orders', orderData)
      commit('ADD_ORDER', response.data)
      return response.data
    } catch (error) {
      throw error
    }
  },
  async cancelOrder({ commit }, id) {
    try {
      await axios.put(`/Orders/${id}/cancel`)
      commit('UPDATE_ORDER_STATUS', { id, status: 'cancelled' })
    } catch (error) {
      throw error
    }
  },
  async confirmReceipt({ commit }, id) {
    try {
      await axios.put(`/Orders/${id}/confirm`)
      commit('UPDATE_ORDER_STATUS', { id, status: 'completed' })
    } catch (error) {
      throw error
    }
  }
}

const getters = {
  orders: state => state.orders,
  currentOrder: state => state.currentOrder
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 