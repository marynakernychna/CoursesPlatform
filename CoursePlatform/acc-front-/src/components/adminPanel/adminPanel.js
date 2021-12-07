import React from 'react';
import { Layout } from 'antd';
import SideMenu from '../sideMenu/index';
import { sectionsNames } from '../../actions/general/sectionsNames';
import { sideMenuTypes } from '../sideMenu/types';
import DataTable from '../admin/dataTable/index';

const { Sider } = Layout

class AdminPanel extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            sectionName: this.props.sectionName
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            sectionName: nextProps.sectionName
        }
    }

    showSectionContent = () => {

        switch (this.state.sectionName) {
            case sectionsNames.COURSES: {
                return <DataTable type={sectionsNames.COURSES}/>
            }
            case sectionsNames.STUDENTS: {
                return <DataTable type={sectionsNames.STUDENTS}/>
            }
        }
    }

    render() {

        return (
            <Layout style={{height: '100vh'}}>
                <Sider style={{paddingTop: '20px', maxHeight: '100%'}}>
                    <SideMenu type={sideMenuTypes.ADMIN}/>
                </Sider>

                {this.showSectionContent()}

            </Layout>
        );
    }
}

export default AdminPanel;