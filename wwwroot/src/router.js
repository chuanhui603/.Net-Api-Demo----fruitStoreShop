import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('./views/Home.vue')
  },
  {
    path: '/products',
    name: 'Products',
    component: () => import('./views/Products.vue')
  },
  {
    path: '/products/:id',
    name: 'ProductDetail',
    component: () => import('./views/ProductDetail.vue')
  },
  {
    path: '/categories',
    name: 'Categories',
    component: () => import('./views/Categories.vue')
  },
  {
    path: '/cart',
    name: 'Cart',
    component: () => import('./views/Cart.vue')
  },
  {
    path: '/checkout',
    name: 'Checkout',
    component: () => import('./views/Checkout.vue')
  },
  {
    path: '/orders',
    name: 'Orders',
    component: () => import('./views/Orders.vue')
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('./views/Login.vue')
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('./views/Register.vue')
  },
  {
    path: '/wishlist',
    name: 'Wishlist',
    component: () => import('./views/Wishlist.vue')
  },
  {
    path: '/member',
    name: 'Member',
    component: () => import('./views/Member.vue'),
    children: [
      {
        path: '',
        redirect: '/member/profile'
      },
      {
        path: 'profile',
        name: 'MemberProfile',
        component: () => import('./views/member/Profile.vue')
      },
      {
        path: 'orders',
        name: 'MemberOrders',
        component: () => import('./views/member/Orders.vue')
      },
      {
        path: 'wishlist',
        name: 'MemberWishlist',
        component: () => import('./views/member/Wishlist.vue')
      },
      {
        path: 'addresses',
        name: 'MemberAddresses',
        component: () => import('./views/member/Addresses.vue')
      },
      {
        path: 'coupons',
        name: 'MemberCoupons',
        component: () => import('./views/member/Coupons.vue')
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('token')
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)

  if (requiresAuth && !isAuthenticated) {
    next({
      path: '/login',
      query: { returnUrl: to.fullPath }
    })
  } else {
    next()
  }
})

export default router 