import React from 'react';
import { Layout } from 'antd';
import Alerts from '../../alert/index';
import Modals from '../../modal/index';
import EclipseWidget from '../../eclipse';
import { sectionsNames } from '../../sideMenu/sectionsNames';
import Courses from './courses/index';

const { Content } = Layout;

class DataTable extends React.Component {

    constructor(props) {
        super(props);
        this.state = {

            isAlert: this.props.isAlert,
            isModal: this.props.isModal,

            loading: this.props.loading,

            isCourses: true
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        if (nextProps.sectionName == sectionsNames.COURSES &&
            prevState.isCourses != true) {

            return {
                isCourses: true
            }
        }
        if (nextProps.sectionName == sectionsNames.STUDENTS &&
            prevState.isCourses != false) {

            return {
                isCourses: false
            }
        }

            return {
                loading: nextProps.loading,
                isAlert: nextProps.isAlert,
                isModal: nextProps.isModal
            }
    }

    render() {

        const { loading, isAlert, isModal, isCourses } = this.state;

        return (

            <Layout style={{ minHeight: 'auto', paddingTop: '20px !important' }}>

                {isAlert && <Alerts />}

                {/* <Content
                    style={{
                        padding: 30,
                        minHeight: '100vh'
                    }}
                > */}

                    {isCourses ? <Courses /> : <>Students</>}

                {/* </Content> */}

                {isModal && <Modals />}

                {loading && <EclipseWidget />}

            </Layout>
        );
    }
}

export default DataTable;