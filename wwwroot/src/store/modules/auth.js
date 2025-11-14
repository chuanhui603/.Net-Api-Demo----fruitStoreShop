import axios from 'axios'
import { jwtDecode } from 'jwt-decode'

const state = {
  token: localStorage.getItem('token') || null,
  expiredDate: JSON.parse(localStorage.getItem('expiredDate')) || null,
  user: JSON.parse(localStorage.getItem('user')) || null
}

const mutations = {
  SET_TOKEN(state, token) {
    state.token = token
    localStorage.setItem('token', token)
  },
  SET_ExpiredDate(state, expiredDate) {
    state.expiredDate = expiredDate
    localStorage.setItem('expiredDate', JSON.stringify(expiredDate))
  },
  SET_USER(state, user) {
    state.user = user
    localStorage.setItem('user', JSON.stringify(user))
  },
  CLEAR_AUTH(state) {
    state.token = null
    state.user = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }
}

const actions = {
  async login({ commit }, credentials) {
    try {
      const response = await axios.post('/api/Auth/login', {
        email: credentials.email,
        password: credentials.password
      })
      const { token, expiration } = response.data
      
      const decodedToken = jwtDecode(token)
      const user = {
        id: decodedToken.jti ,
        email: decodedToken.email,
        name: decodedToken.sub 
      }


      commit('SET_TOKEN', token)
      commit('SET_ExpiredDate', expiration)
      commit('SET_USER', user)
      return { token, expiration, user }
    } catch (error) {
      throw error
    }
  },

  
  async register({ commit }, userData) {
    try {
      const response = await axios.post('/api/Customer',userData )
      if (response.status !== 204) {
        throw new Error('註冊失敗，請稍後再試')
      }
      const mockUser = {
        id: Date.now(),
        ...userData
      }
      const mockToken = 'mock-jwt-token-for-testing'

      commit('SET_TOKEN', mockToken)
      commit('SET_USER', mockUser)
      return { token: mockToken, user: mockUser }
    } catch (error) {
      throw error
    }
  },
  logout({ commit }) {
    commit('CLEAR_AUTH')
  }
}

const getters = {
  isAuthenticated: state => !!state.token,
  currentUser: state => state.user
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 