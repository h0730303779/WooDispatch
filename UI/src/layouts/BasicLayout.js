import * as React from 'react';
import styles from './index.css';
import { Layout, Menu, Icon } from 'antd';
import Link from 'umi/link';
// import zhCN from 'antd/lib/locale-provider/zh_CN';
import moment from 'moment';
import 'moment/locale/zh-cn';

moment.locale('zh-cn');
const { Header,Content,Footer } = Layout;




class BasicLayout extends React.Component {
  state = {
    current: this.props.location.pathname,
  }

  handleClick = (e) => {
    //console.log('click ', e);
    this.setState({
      current: e.key,
    });
  }

  render(){
    return (
      // <div className={styles.normal}>
      //   <h1 className={styles.title}>Yay! Welcome to umi!</h1>
      //    <Menu.Item key="/kuaidi"><Link to='/kuaidi'>快递单号查询</Link> </Menu.Item>
      // </div>
      <Layout className="layout" style={{height: '100%'}}>
          <Header >
            <div className={styles.logo} >任务调度平台</div>
            
            <Menu
            theme="dark"
            mode="horizontal"
            onClick={this.handleClick}
            selectedKeys={[this.state.current]}
            mode="horizontal"
            style={{ lineHeight: '64px' }}
            >
              <Menu.Item key="/"><Link to='/'><span><Icon type="build" /></span>任务列表</Link></Menu.Item>
              <Menu.Item key="/conver"><Link to='/conver'>关于</Link> </Menu.Item>
              
           </Menu>
          </Header>
          <Content style={{ padding: '0 50px' }}>
            <div style={{margin:'10px 0 0 0', minHeight: 360}}>
              {this.props.children}
            </div>
          </Content>
          <Footer style={{ textAlign: 'center',padding:'0'}}>
            乌云科技 ©2018 Created by  苏ICP备17034003号-2
          </Footer>
        </Layout>
    );
  }
}

export default BasicLayout;
