import React from 'react';
import { Layout, Spin } from 'antd';
import SideMenu from '../sideMenu/index';
import { sideMenuTypes } from '../sideMenu/types';
import Modals from '../modal/index';
import Alerts from '../alert/index';

const { Sider } = Layout;
const { Content } = Layout;

class AdminPanelPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = {

            isAlert: this.props.isAlert,
            isModal: this.props.isModal,

            loading: this.props.loading
        };
    }

    componentDidMount() {
        document.title = "Courses";
    };

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            loading: nextProps.loading,
            isAlert: nextProps.isAlert,
            isModal: nextProps.isModal
        }
    }

    render() {

        const { loading, isAlert, isModal } = this.state;

        return (

            <Layout style={{ minHeight: '100vh' }}>

                <Sider style={{ paddingTop: '20px', maxHeight: '100%' }}>
                    <SideMenu type={sideMenuTypes.ADMIN} />
                </Sider>

                <Layout className="site-layout">

                    <Spin size="large" spinning={loading}>

                        <div>
                            {isAlert && <Alerts />}

                            <Content
                                className="site-layout-background"
                                style={{
                                    padding: 30,
                                }}
                            >

                                {this.props.children}

                            </Content>

                            {isModal && <Modals />}

                        </div>

                    </Spin>

                </Layout>

            </Layout>
        );
    }
}

export default AdminPanelPage;