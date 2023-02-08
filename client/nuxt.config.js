module.exports = {
  // Disable server-side rendering: https://go.nuxtjs.dev/ssr-mode
  ssr: false,

  // Target: https://go.nuxtjs.dev/config-target
  target: 'static',

  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    title: 'Báo cáo sản xuất QSP',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/logo-hp.ico' },
      { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
      { rel: 'preconnect', href: 'https://fonts.gstatic.com' },
      { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap'},
      { rel: 'stylesheet', href: 'https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css'},
    ],
    script: [
      {
        src:
          "https://code.jquery.com/jquery-3.3.1.slim.min.js",
          type: "text/javascript"
      },
      {
        src:
          "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js",
          type: "text/javascript"
      },
      {
        src:
          "https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js",
          type: "text/javascript"
      },
      {
        src:
          "https://kit.fontawesome.com/073b9166e3.js",
          type: "text/javascript"
      },
    ],
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [
    '~/assets/scss/main.scss',
    'vue2-datepicker/index.css'
    // '~/node_modules/bootstrap/dist/css/bootstrap.css'
  ],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
    { src: '~/plugins/vue2-datepicker', ssr: false },
    { src: '~/plugins/v-click-outside', ssr: false },
  ],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    '@nuxtjs/style-resources',
    '@nuxtjs/moment',
  ],
  styleResources: {
    // your settings here
    sass: [],
    scss: [
      '~/assets/scss/main.scss',
    ],
    hoistUseStatements: true  // Hoists the "@use" imports. Applies only to "sass", "scss" and "less". Default: false.
  },

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/bootstrap
    'bootstrap-vue/nuxt',
    '@nuxtjs/axios',
    '@nuxtjs/proxy',
  ],

  axios: {
    proxy: true,
  },
  proxy: {
    '/api/': {
      target: 'https://localhost:44315',
      // target: 'http://192.168.103.210:9000',
      pathRewrite: { '^/api/': '/api/' },
      changeOrigin: true,
      secure: false,
    },
  },
  build: {
  },
  env: {
    baseUrl : process.env.BASE_URL || 'https://localhost:44315',
    baseApiUrl: process.env.BASE_API_URL || 'https://localhost:44315/api/',
    // baseUrl : process.env.BASE_URL || 'http://192.168.103.210:9000',
    // baseApiUrl: process.env.BASE_API_URL || 'http://192.168.103.210:9000/api/',
  },
  // router: {
  //   middleware: 'auth',
  // }
}
