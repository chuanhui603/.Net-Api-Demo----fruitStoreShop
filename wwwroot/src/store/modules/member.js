import axios from 'axios'

const state = {
  memberInfo: null,
  members: [],
  loading: false,
  error: null
}

const mutations = {
  SET_MEMBER_INFO(state, member) {
    state.memberInfo = member
  },
  SET_MEMBERS(state, members) {
    state.members = members
  },
  UPDATE_MEMBER(state, updatedMember) {
    state.memberInfo = updatedMember
    const index = state.members.findIndex(m => m.id === updatedMember.id)
    if (index !== -1) {
      state.members.splice(index, 1, updatedMember)
    }
  },
  ADD_MEMBER(state, newMember) {
    state.members.push(newMember)
  },
  SET_LOADING(state, loading) {
    state.loading = loading
  },
  SET_ERROR(state, error) {
    state.error = error
  }
}

const actions = {
  async register({ commit }, memberData) {
    commit('SET_LOADING', true)
    commit('SET_ERROR', null)
    try {
        console.log('Creating member with data:', memberData)
      const response = await axios.post('/api/Customer/Create', memberData)
      commit('ADD_MEMBER', response.data)
      commit('SET_LOADING', false)
      return response.data
    } catch (error) {
      commit('SET_ERROR', error.response?.data?.message || '新增會員失敗')
      commit('SET_LOADING', false)
      throw error
    }
  },

  // 更新會員資料
  async updateMember({ commit }, memberData) {
    commit('SET_LOADING', true)
    commit('SET_ERROR', null)
    try {
      const response = await axios.put(`/api/members/${memberData.id || ''}`, memberData)
      commit('UPDATE_MEMBER', response.data)
      commit('SET_LOADING', false)
      return response.data
    } catch (error) {
      commit('SET_ERROR', error.response?.data?.message || '更新會員失敗')
      commit('SET_LOADING', false)
      throw error
    }
  },

  // 獲取會員資料
  async fetchMember({ commit }, memberId) {
    commit('SET_LOADING', true)
    commit('SET_ERROR', null)
    try {
      const response = await axios.get(`/api/members/${memberId}`)
      commit('SET_MEMBER_INFO', response.data)
      commit('SET_LOADING', false)
      return response.data
    } catch (error) {
      commit('SET_ERROR', error.response?.data?.message || '獲取會員資料失敗')
      commit('SET_LOADING', false)
      throw error
    }
  },

  // 獲取所有會員列表
  async fetchMembers({ commit }, params = {}) {
    commit('SET_LOADING', true)
    commit('SET_ERROR', null)
    try {
      const response = await axios.get('/api/members', { params })
      commit('SET_MEMBERS', response.data)
      commit('SET_LOADING', false)
      return response.data
    } catch (error) {
      commit('SET_ERROR', error.response?.data?.message || '獲取會員列表失敗')
      commit('SET_LOADING', false)
      throw error
    }
  },

  // 刪除會員
  async deleteMember({ commit, state }, memberId) {
    commit('SET_LOADING', true)
    commit('SET_ERROR', null)
    try {
      await axios.delete(`/api/members/${memberId}`)
      const updatedMembers = state.members.filter(m => m.id !== memberId)
      commit('SET_MEMBERS', updatedMembers)
      commit('SET_LOADING', false)
    } catch (error) {
      commit('SET_ERROR', error.response?.data?.message || '刪除會員失敗')
      commit('SET_LOADING', false)
      throw error
    }
  }
}

const getters = {
  memberInfo: state => state.memberInfo,
  members: state => state.members,
  loading: state => state.loading,
  error: state => state.error,
  getMemberById: (state) => (id) => {
    return state.members.find(member => member.id === id)
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 