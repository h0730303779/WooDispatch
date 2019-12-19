import * as React from 'react'
import { Helmet } from "react-helmet";
// import  '../../assets/KDNWidget/KDNWidget.js'
// import '../../assets/KDNWidget/KDNWidget.css';
export default class kuaidi extends React.Component{
    // constructor(props){
    //     super(props);
    //     // KDNWidget.run({
    //     //     "serviceType": "D",
    //     //     "container": "demoID",
    //     // })
    // }

    componentDidMount(){

        // const s = document.createElement('script');
        // s.type = 'text/javascript';
        // s.src = 'http://www.kdniao.com/OutDemo/KDNWidget/KDNWidget.js';
        // s.async = true;
        
        // s.onload= function () {
           
        //     const { KDNWidget } = window;
        //      KDNWidget.run({
        //         "serviceType": "D",
        //         "container": "demoID",
        //      })
        // }    
        // document.body.appendChild(s);
        const { KDNWidget } = window;
        console.log(KDNWidget);
                  
            //  KDNWidget.run({
            //     "serviceType": "D",
            //     "container": "demoID",
            //  })
        // var xhr = new XMLHttpRequest();
        // xhr.onreadystatechange = function() {//服务器返回值的处理函数，此处使用匿名函数进行实现
        //     if (xhr.readyState == 4 && xhr.status == 200) {//
        //         var responseText = xhr.responseText;
        //         //document.getElementById('targetlist').innerHTML = responseText;
        //         console.log(responseText);
        //     }
        // };
        // xhr.open("GET", "/wxroot/JS/home", true);//提交get请求到服务器
        // xhr.send(null);
    }

    render(){

        return (
            <div>
                <Helmet>
                    <title>快递单号查询</title>
                </Helmet>
            
                <div id="demoID"></div>
            
            </div>
        );
    }
};