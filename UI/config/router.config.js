
export default [
    
  { 
    path: '/Dispatch', 
    component: '../layouts/BasicLayout',
    routes: [
      { path: '/Dispatch', redirect: '/Dispatch/JobHome'},
      { path: '/Dispatch/JobHome', component: './home/home' },
      { path: '/Dispatch/conver',component:'./home/conver'},
      // { path: '/kuaidi',component:'./home/kuaidi'},
      // { path: '/demo', component: './demo/demo1' },
      { component: '404'},
    ]
  },
];