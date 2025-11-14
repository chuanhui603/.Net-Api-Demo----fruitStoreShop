import axios from 'axios'

const state = {
  list: [],
  current: null,
  featured: [],
  reviews: [],
  total: 0
}

const mutations = {
  SET_PRODUCTS(state, { products, total }) {
    state.list = products
    state.total = total
  },
  SET_CURRENT_PRODUCT(state, product) {
    state.current = product
  },
  SET_FEATURED_PRODUCTS(state, products) {
    state.featured = products
  },
  SET_PRODUCT_REVIEWS(state, reviews) {
    state.reviews = reviews
  },
  ADD_PRODUCT_REVIEW(state, review) {
    state.reviews.push(review)
  }
}

const actions = {
  async fetchProducts({ commit }, params) {
    try {
      // 測試用假資料
      const mockProducts = [
        {
          id: 1,
          name: '台灣鳳梨',
          description: '新鮮多汁的台灣鳳梨，甜度高，果肉細緻',
          price: 150,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 2,
          name: '日本富士蘋果',
          description: '日本進口富士蘋果，脆甜多汁，果肉細緻',
          price: 200,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 3,
          name: '台灣香蕉',
          description: '台灣本土香蕉，香氣濃郁，口感綿密',
          price: 80,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 4,
          name: '智利櫻桃',
          description: '智利進口櫻桃，果實飽滿，甜度高',
          price: 350,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 5,
          name: '台灣芒果',
          description: '台灣愛文芒果，香氣濃郁，果肉細緻',
          price: 180,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 6,
          name: '紐西蘭奇異果',
          description: '紐西蘭進口奇異果，酸甜適中，營養豐富',
          price: 120,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 7,
          name: '台灣蓮霧',
          description: '台灣黑珍珠蓮霧，清脆多汁，甜度高',
          price: 160,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 8,
          name: '美國葡萄',
          description: '美國進口葡萄，果實飽滿，甜度高',
          price: 280,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        }
      ]

      // 根據分類過濾
      let filteredProducts = mockProducts
      if (params.category) {
        filteredProducts = mockProducts.filter(p => p.categoryId === parseInt(params.category))
      }

      // 根據價格範圍過濾
      if (params.minPrice) {
        filteredProducts = filteredProducts.filter(p => p.price >= parseInt(params.minPrice))
      }
      if (params.maxPrice) {
        filteredProducts = filteredProducts.filter(p => p.price <= parseInt(params.maxPrice))
      }

      // 根據搜索關鍵字過濾
      if (params.search) {
        const searchLower = params.search.toLowerCase()
        filteredProducts = filteredProducts.filter(p => 
          p.name.toLowerCase().includes(searchLower) || 
          p.description.toLowerCase().includes(searchLower)
        )
      }

      // 排序
      if (params.sortBy) {
        const [field, order] = params.sortBy.split('_')
        filteredProducts.sort((a, b) => {
          if (order === 'asc') {
            return a[field] - b[field]
          } else {
            return b[field] - a[field]
          }
        })
      }

      // 分頁
      const page = params.page || 1
      const pageSize = params.pageSize || 9
      const start = (page - 1) * pageSize
      const end = start + pageSize
      const paginatedProducts = filteredProducts.slice(start, end)

      commit('SET_PRODUCTS', {
        products: paginatedProducts,
        total: filteredProducts.length
      })
    } catch (error) {
      console.error('Error fetching products:', error)
      commit('SET_PRODUCTS', {
        products: [],
        total: 0
      })
    }
  },
  async fetchProduct({ commit }, id) {
    try {
      // 測試用假資料
      const mockProducts = [
        {
          id: 1,
          name: '台灣鳳梨',
          description: '新鮮多汁的台灣鳳梨，甜度高，果肉細緻',
          price: 150,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        // ... 其他商品
      ]
      const product = mockProducts.find(p => p.id === parseInt(id))
      commit('SET_CURRENT_PRODUCT', product)
    } catch (error) {
      throw error
    }
  },
  async fetchFeaturedProducts({ commit }) {
    try {
      // 測試用假資料
      const mockFeaturedProducts = [
        {
          id: 1,
          name: '台灣鳳梨',
          description: '新鮮多汁的台灣鳳梨，甜度高，果肉細緻',
          price: 150,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 2,
          name: '日本富士蘋果',
          description: '日本進口富士蘋果，脆甜多汁，果肉細緻',
          price: 200,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 3,
          name: '台灣香蕉',
          description: '台灣本土香蕉，香氣濃郁，口感綿密',
          price: 80,
          categoryId: 1,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        },
        {
          id: 4,
          name: '智利櫻桃',
          description: '智利進口櫻桃，果實飽滿，甜度高',
          price: 350,
          categoryId: 3,
          imageUrl: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
        }
      ]
      commit('SET_FEATURED_PRODUCTS', mockFeaturedProducts)
    } catch (error) {
      throw error
    }
  },
  async fetchReviews({ commit }, productId) {
    try {
      const response = await axios.get(`/Products/${productId}/reviews`)
      commit('SET_PRODUCT_REVIEWS', response.data)
    } catch (error) {
      throw error
    }
  },
  async submitReview({ commit }, { productId, rating, content }) {
    try {
      const response = await axios.post(`/Products/${productId}/reviews`, {
        rating,
        content
      })
      commit('ADD_PRODUCT_REVIEW', response.data)
      return response.data
    } catch (error) {
      throw error
    }
  }
}

const getters = {
  products: state => state.list,
  currentProduct: state => state.current,
  featuredProducts: state => state.featured,
  productReviews: state => state.reviews,
  totalProducts: state => state.total
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
} 