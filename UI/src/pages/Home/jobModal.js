
import * as React from 'react';
import {
    Modal, Form, Spin, TimePicker, Button, Input, Icon, Checkbox, Select, Radio, Tabs
} from 'antd';

import localForm from './localForm';
import httpForm from './httpForm';
import request from '../../utils/request';
const { TabPane } = Tabs;


// const rangeConfig = {
//     rules: [{ type: 'array', required: true, message: 'Please select time!' }],
// };

// const config = {
//     rules: [{ type: 'object', required: true, message: 'Please select time!' }],
// };

// const antdFromsitemtyle = {
//     marginBottom: { marginBottom: '8px' }
// }

const WrappedlocalForm = Form.create({ name: 'localForm' })(localForm);
const WrappedhttpForm = Form.create({ name: 'httpform' })(httpForm);

export default class jobModal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            activeKey: "1",
            confirmLoading:false
        }
    }

      //初始化事件
  componentDidMount() {

  }



    /**
     * tabs切换事件
    */
   onChange = (activeKey) => {
        console.log(activeKey);
        //this.state.activeKey = activeKey;
        this.setState({ activeKey });
    }

    /**
     * 设置表单
    */
    putform=(editdata)=>{
        if(!editdata) return;
        debugger;
        if(editdata.targetType ==="1")
        {
            // let obj = {} 
            // Object.getOwnPropertyNames(data).forEach(item =>{

            //     console.log(item,data[item]);
                
            // })
            this.httpForm.setFieldsValue(editdata);
            
        }
        else if(editdata.targetType ==="2")
        {
            // let obj = {} 
            // Object.getOwnPropertyNames(data).forEach(item =>{

            //     console.log(item,data[item]);
                
            // })
            this.localForm.setFieldsValue(editdata);
            
        }
        else if(editdata.targetType ==="3")
        {

        }
        else 
        {
            // let obj = {} 
            // Object.getOwnPropertyNames(data).forEach(item =>{

            //     console.log(item,data[item]);
                
            // })
            this.localForm.setFieldsValue(editdata);
            
        }
    }

    /**
     * 
    */
    onok=()=>{

        const { activeKey } = this.state;
        this.setState({ confirmLoading: true, });
        
        // debugger;
        // // this.props.makethatState(true);
        // //
        // console.log(this.state.activeKey);
        if (activeKey == "1") {
            const form = this.httpForm;
            form.validateFields((err, values) => {
                if (err) {
                    this.setState({ confirmLoading: false });
                    return;
                }

                var requestUrl = "";
                if(this.props.isEditstate)
                {
                    requestUrl = "/Dispatch/jobs/modifyJob";
                   
                }
                else  {
                    requestUrl = "/Dispatch/jobs/AddJob";
                }

                request(requestUrl, {
                    method: 'post',
                    body: values
                }).then(data => {
                    this.setState({ confirmLoading: false });
                    this.props.handleOk(data);
                });
            });
        }
        else if (activeKey == "2") {
            const form = this.localForm;
            form.validateFields((err, values) => {
                if (err) {
                    this.setState({ confirmLoading: false });
                    return;
                }

                var requestUrl = "";
                if(this.props.isEditstate)
                {
                    requestUrl = "/Dispatch/jobs/modifyJob";
                   
                }
                else  {
                    requestUrl = "/Dispatch/jobs/AddJob";
                }

                request(requestUrl, {
                    method: 'post',
                    body: values
                }).then(data => {
                    this.setState({ confirmLoading: false });
                    this.props.handleOk(data);
                });
            });
        }
        //this.props.handleOk();
    }

    render() {


        const { visible } = this.props;
        const { handleOk, handleCancel,editdata,loading } = this.props;
        
        // if(JSON.stringify(this.props.editdata) == "{}") 
        // {
        //     console.log("新增")
        // }else
        // { console.log("编辑");
        // }
        return (
            <>
                <Modal

                    confirmLoading={this.state.confirmLoading}
                    visible={visible}
                    onOk={this.onok}
                    onCancel={handleCancel}
                    okText={"提交"}
                    cancelText="取消"
                    maskClosable={false}
                    width={570}
                    bodyStyle={{ padding: '12px' }}
                    destroyOnClose = {true}
                >
                <Spin spinning={loading}>
        
        
                    <Tabs activeKey={this.state.activeKey} onChange={this.onChange}>
                        <TabPane tab="普通业务" key="1">

                            <WrappedhttpForm 
                            ref={(form) => this.httpForm = form}
                            MakeMoney={this.MakeMoney}
                            />
                        </TabPane>
                        <TabPane tab="自定义业务" key="2">
                            <WrappedlocalForm ref={(form) => this.localForm = form} />
                        </TabPane>
                        <TabPane tab="预制业务" key="3">
                            null
                    </TabPane>
                    </Tabs>

                </Spin>
  </Modal>
            </>
        )
    }
}