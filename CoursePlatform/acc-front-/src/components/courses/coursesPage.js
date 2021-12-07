import React from 'react';
import { Layout } from 'antd';
import Alerts from '../alert/index';
import Modals from '../modal/index';
import SortMenu from '../sortMenu/index';
import EclipseWidget from '../eclipse';
import { sectionsNames } from '../sideMenu/sectionsNames';
import Subscriptions from './subscriptions/index';
import AllCourses from './allCourses/index';
import PagePagination from '../pagination/index';

const { Content } = Layout;

class CoursesPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = {

            isAlert: this.props.isAlert,
            isModal: this.props.isModal,

            loading: this.props.loading,

            isAllCourses: true
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        if (nextProps.sectionName == sectionsNames.COURSES &&
            prevState.isAllCourses != true) {

            return {
                isAllCourses: true
            }
        }
        if (nextProps.sectionName == sectionsNames.SUBSCRIPTIONS &&
            prevState.isAllCourses != false) {

            return {
                isAllCourses: false
            }
        }

            return {
                loading: nextProps.loading,
                isAlert: nextProps.isAlert,
                isModal: nextProps.isModal
            }
    }

    render() {

        const { loading, isAlert, isModal, isAllCourses } = this.state;

        return (

            <Layout style={{ minHeight: '100vh', paddingTop: '20px !important' }}>

                {isAlert && <Alerts />}

                <Content
                    style={{
                        padding: 30,
                        minHeight: 280
                    }}
                >
                    <SortMenu />

                    {isAllCourses ? <AllCourses /> : <Subscriptions />}

                    <PagePagination />

                </Content>

                {isModal && <Modals />}

                {loading && <EclipseWidget />}

            </Layout>
        );
    }
}

export default CoursesPage;