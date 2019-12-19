import pageRoutes from './router.config';

// ref: https://umijs.org/config/
export default {
  treeShaking: true,
  plugins: [
    // ref: https://umijs.org/plugin/umi-plugin-react.html
    ['umi-plugin-react', {
      antd: true,
      dva: true,
      dynamicImport: false,
      title: 'APP',
      dll: false,

    }],
  ],
  targets: {
    ie: 11,
  },

  // proxy: {
  //   '/api': {
  //     target: "http://127.0.0.5000/",
  //     changeOrigin: true,
  //     pathRewrite: { "^/api" : "" }
  //   },
  // },

  proxy: {
    '/Dispatch/': {
      target: 'http://localhost:5000',
      pathRewrite: { '^/Dispatch': '/Dispatch' },
      changeOrigin: true
    },
  },
  // exportStatic: {
  //   htmlSuffix: true
  // }, //如果设为 true 或 Object，则导出全部路由为静态页面，否则默认只输出一个 index.html。

  routes: pageRoutes,
  ignoreMomentLocale: true

}