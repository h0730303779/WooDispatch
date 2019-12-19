import * as React from 'react';
import {
    Modal, Form, DatePicker, TimePicker, Button, Input, Icon, Checkbox, Select, Radio, Tabs
} from 'antd';

import moment from 'moment';

const { TextArea } = Input;
const antdFromsitemtyle = {
    marginBottom: { marginBottom: '8px' }
}
export default class httpForm extends React.Component {
    state = {
        value: 1,
    };

    onChange = e => {
        this.setState({
            value: e.target.value,
        });
    };

    render() {
        const { getFieldDecorator } = this.props.form;
        const formItemLayout = {
            labelCol: {
                xs: { span: 24 },
                sm: { span: 8 },
            },
            wrapperCol: {
                xs: { span: 24 },
                sm: { span: 16 },
            },
        };
        return (<>

            <Form {...formItemLayout} labelCol={{ span: 5 }} wrapperCol={{ span: 12 }}>
                <Form.Item style={antdFromsitemtyle.marginBottom} label="任务组名" >
                    {getFieldDecorator('jobGroup', {
                        rules: [{ required: true, message: '请输入任务组名!' }],
                    })(
                        <Input placeholder="请输入任务组名" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="任务名称" >
                    {getFieldDecorator('jobName', {
                        rules: [{ required: true, message: '请输入任务名!' }],
                    })(
                        <Input placeholder="请输入任务名" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="请求地址" >
                    {getFieldDecorator('requestUrl', {
                        rules: [{ required: true, message: '请输入对象名称!' }],
                    })(
                        <Input placeholder="对象名称" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="起始时间" >
                    {getFieldDecorator('beginTime', {
                        rules: [{ type: 'object', required: true, message: 'Please select time!' }],
                        initialValue: null,
                        normalize: (value) => {
                            if(!value) return null;
                        return moment(value, 'YYYY-MM-DD HH:mm:ss')   ;
                            }
                    })(
                        <DatePicker placeholder="起始时间" showTime format="YYYY-MM-DD HH:mm:ss" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="终止时间" >
                    {getFieldDecorator('endTime', {
                        rules: [{ type: 'object', message: 'Please select time!' }]
                        ,
                        initialValue: null,
                        normalize: (value) => {
                            if(!value) return null;
                            return moment(value, 'YYYY-MM-DD HH:mm:ss')   ;        
                        }
                    })(
                        <DatePicker placeholder="终止时间" showTime format="YYYY-MM-DD HH:mm:ss" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="Cron表达式" >
                    {getFieldDecorator('cron', {
                        rules: [{ required: true, message: '请输入Cron表达式!' }],
                    })(
                        <Input placeholder="Cron表达式" />
                    )}
                </Form.Item>
                <Form.Item style={antdFromsitemtyle.marginBottom} label="请求类型" >
                    {getFieldDecorator('requestType', {
                        rules: [{ required: true, message: '请求类型必填' }],
                        initialValue: 1
                    })(
                        <Radio.Group onChange={this.onChange}>
                            <Radio value={1}>Get</Radio>
                            <Radio value={2}>Post</Radio>
                            <Radio value={3}>Put</Radio>
                            <Radio value={4}>Delete</Radio>
                        </Radio.Group>
                    )}
                </Form.Item>
                <Form.Item style={antdFromsitemtyle.marginBottom} label="请求头" >
                    {getFieldDecorator('headers')(
                        <TextArea rows={2} placeholder="请求头" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="请求参数" >
                    {getFieldDecorator('requestParameters')(
                        <TextArea rows={2} placeholder="请求参数" />
                    )}
                </Form.Item>

                <Form.Item style={antdFromsitemtyle.marginBottom} label="任务描述" >
                    {getFieldDecorator('description')(
                        <TextArea rows={2} placeholder="任务描述" />
                    )}
                </Form.Item>

                <Form.Item>
                    {getFieldDecorator('triggerType', { initialValue: "Cron" })}
                </Form.Item>
                <Form.Item>
                    {getFieldDecorator('targetType', { initialValue: "1" })}
                </Form.Item>
            </Form>
        </>)
    }
}