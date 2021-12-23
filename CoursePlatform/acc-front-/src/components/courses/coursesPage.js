import React from 'react';
import { Layout, Spin } from 'antd';
import Alerts from '../alert/index';
import Modals from '../modal/index';
import SideMenu from '../sideMenu/index';
import { sideMenuTypes } from '../sideMenu/types';

const { Content } = Layout;
const { Sider } = Layout;

class CoursesPage extends React.Component {

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
                    <SideMenu type={sideMenuTypes.STUDENT} />
                </Sider>

                <Layout style={{ minHeight: '100vh', paddingTop: '20px !important' }}>

                    <Spin size="large" spinning={loading}>

                        <div>

                            {isAlert && <Alerts />}

                            {this.props.children}

                            {isModal && <Modals />}

                        </div>

                    </Spin>

                </Layout>

            </Layout>
        );
    }
}

export default CoursesPage;