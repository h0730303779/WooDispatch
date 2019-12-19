import * as React from 'react';
import { Modal, Card, Divider, Form, Button, Table, Breadcrumb, message, Badge, Icon } from 'antd';
import request from '../../utils/request';
import JobModal from './jobModal';
import moment from 'moment'; // 时间插件


const iconstyle = {
  color: { color: '#54698d' }
}


// const WrappedJobModal = Form.create({ name: 'register' })(JobModal);
export default class Home extends React.Component {

  /**
   * 开启
  */
  actrun = (record) => {
    Modal.confirm({
      title: '确认',
      content: '您确认要启动[' + record.name + ']任务吗？',
      onOk: () => {
        request("/Dispatch/jobs/jobrun", {
          method: 'post',
          body: { Group: record.groupName, Name: record.name }
        }).then(data => {
          if (data.msgCode === 1) {
            message.success(data.msg);
            this.query();
          }
          else {
            message.error(data.msg);
          }

        })
        //message.success('删除成功');
        //this.request();
      }
    })
  }

  /**
   * 暂停
  */
  actstop = (record) => {
    Modal.confirm({
      title: '确认',
      content: '您确认要停止[' + record.name + ']任务吗？',
      onOk: () => {
        request("/Dispatch/jobs/jobstop", {
          method: 'post',
          body: { Group: record.groupName, Name: record.name }
        }).then(data => {

          if (data.msgCode === 1) {
            message.success(data.msg);
            this.query();
          }
          else {
            message.error(data.msg);
          }

        })
        //message.success('删除成功');
        //this.request();
      }
    })
  }

  /**
   * 删除任务
  */
  actdel = (record) => {
    Modal.confirm({
      title: '确认',
      content: '您确认要删除[' + record.name + ']任务吗？',
      onOk: () => {
        request("/Dispatch/jobs/jobdelete", {
          method: 'post',
          body: { Group: record.groupName, Name: record.name }
        }).then(data => {

          if (data.msgCode === 1) {
            message.success(data.msg);
            this.query();
          }
          else {
            message.error(data.msg);
          }

        })
        //message.success('删除成功');
        //this.request();
      }
    })
  }

  /**
   * 执行一次 
  */
  actfistrun = (record) => {
    Modal.confirm({
      title: '确认',
      content: '您确认要执行[' + record.name + ']任务吗？',
      onOk: () => {
        request("/Dispatch/jobs/TriggerJob", {
          method: 'post',
          body: { Group: record.groupName, Name: record.name }
        }).then(data => {
          if (data === true)
            message.success("执行了一次此任务");
          else
            message.error('失败原因未知');
        })
        //message.success('删除成功');
        //this.request();
      }
    })
  }

  state = {
    tabdata: [], // Check here to configure the default column
    loading: false,
    Modalvisible: false,
    confirmLoading: false,
    isEditstate:false
  };

  /**
   * 编辑任务
  */
  actedit=(record)=>{
    this.setState({
      Modalvisible: true,
      loading:true,
      isEditstate:true,
    });
    debugger;
    this.jobModal.onChange(record.targetType || "2");

    request("/Dispatch/jobs/GetTaskEntiy", {
      method: 'post',
      body: { Group: record.groupName, Name: record.name }
    }).then(data => {
      this.jobModal.putform(data);
      this.setState({
        loading:false,
      });
    })
  }

  onRef = (ref) => {
    debugger;
           this.jobModal = ref
  }

  //初始化事件
  componentDidMount() {
    this.query();
  }

  query() {
    request("/Dispatch/jobs", {
      method: 'get',
    }).then(data => {
      this.setState({ tabdata: data });
    })
  }

  /**
   * 添加任务
  */
  btnadd = () => {
    this.setState({
      Modalvisible: true,
      loading:false,
      isEditstate:false
    });

  }


  /**
   * 调度器 开启
  */
  btnstartSchedule = () => {
    request("/Dispatch/jobs/startSchedule", {
      method: 'get',
    }).then(data => {

    })

  }

  /**
   * 调度器 关闭
  */
  btnstopSchedule = () => {
    request("/Dispatch/jobs", {
      method: 'get',
    }).then(data => {

    })

  }




  /**
   * 提交表单数据
   * */
  handleOk = (data) => {

    if (data.msgCode == 1) {
      message.success(data.msg);
      this.setState({
        Modalvisible: false,
      });
      this.query();
    }
    else if (data.msgCode == 2) {
      message.error(data.msg);
    }
    else {
      message.warning(data.msg);
    }


  }



  handleCancel = (e) => {
    this.setState({
      Modalvisible: false,
    });
  }

  render() {
    // const { loading, selectedRowKeys } = this.state;

    const btnstyle = {
      margin: '0 7px',
      backgroundColor: '#54698d',
      borderColor: '#54698d'
    }

    const columns = [
      { title: '任务名称', dataIndex: 'name' },
      {
        title: '状态', dataIndex: 'triggerState', align: 'center', render: text => {
          if (text == '0') {
            return <Badge status="processing" />;
          }
          else {
            return <Badge status="default" />;
          }
        }
      },
      { title: '执行地址', dataIndex: 'triggerCall' }
      , { title: "类型", dataIndex: 'targetType', render:text=>{
        if (text == '1') {
          return <span>普通业务</span>;
        }
        else if (text == '2'){
          return <span>自定义业务</span>;
        }
        else if (text == '3')
        {
          return <span>预制业务</span>;
        }
        else 
          return <span>自定义业务</span>;
      }}
      , { title: "开始时间", dataIndex: 'beginTime', render: val => <span>{moment(val).format('YYYY-MM-DD HH:mm:ss')}</span> }
      , { title: "上次执行时间", dataIndex: 'previousFireTime', render: val => <span>{moment(val).format('YYYY-MM-DD HH:mm:ss')}</span> }
      , { title: "下次执行时间", dataIndex: 'nextFireTime', render: val => <span>{moment(val).format('YYYY-MM-DD HH:mm:ss')}</span> }
      // , { title: "执行计划", dataIndex: '执行计划' }
      , { title: "描述", dataIndex: 'description' }
      , {
        title: '操作',

        render: (text, record) => {

          return (
            <>
              <a  title="编辑"  onClick={(item) => { this.actedit(record) }}><Icon type="edit" style={iconstyle.color} /></a>
              <Divider type="vertical" />
              { record.triggerState == '0' ? (
                <a  title="暂停" onClick={(item) => { this.actstop(record) }} ><Icon type="pause" style={iconstyle.color} /></a>
              ):(
                <a  title="启动" onClick={(item) => { this.actrun(record) }} ><Icon type="caret-right" style={iconstyle.color} /></a>
              )}
              
              <Divider type="vertical" />
              <a  title="立刻执行一次"  onClick={(item) => { this.actfistrun(record) }}><Icon type="right-circle" style={iconstyle.color} /></a>
              <Divider type="vertical" />
              <a  title="删除" onClick={(item) => { this.actdel(record) }}><Icon type="delete" style={iconstyle.color} /></a>


              {/* <Button size="small" onClick={(item) => { this.actrun(record) }}>启动</Button>
              <br />
              <Button size="small" onClick={(item) => { this.actstop(record) }}>暂停</Button>
              <br />
              <Button size="small" onClick={(item) => { this.actfistrun(record) }}>执行一次</Button>
              <br />
               */}
            </>
          )
        }

      }
    ];
    // const hasSelected = selectedRowKeys.length > 0;
    return (
      <>

        <JobModal
          ref = {(ref)=> this.jobModal = ref}
          // onRef={this.onRef}
          visible={this.state.Modalvisible}
          handleOk={this.handleOk}
          handleCancel={this.handleCancel} 
          isEditstate = {this.state.isEditstate}
          loading = {this.state.loading}
          />
        <Breadcrumb style={{ margin: '16px 0' }}>
          <Breadcrumb.Item>任务列表</Breadcrumb.Item>
        </Breadcrumb>

        <Card>
          <div>
            <div style={{ marginBottom: 16, }}>
              <Button
                type="primary"
                style={btnstyle}
                onClick={this.btnadd}
              >
                新建任务
                  </Button>
              {/* <Button
                type="primary"
                style={btnstyle}
                onClick={this.btnstartSchedule}
              >
                调度器启动
                  </Button>
              <Button
                type="primary"
                style={btnstyle}
                onClick={this.btnstopSchedule}
              >
                调度器停止
                  </Button> */}
            </div>
            <Table
              // rowSelection={rowSelection}
              columns={columns}
              dataSource={this.state.tabdata} />
          </div>
        </Card>
      </>
    );
  }
}
