import React, { Fragment } from 'react';
import { Menu } from 'antd';
import {
    DesktopOutlined,
    AppstoreOutlined,
    ExportOutlined
} from '@ant-design/icons';
import { sectionsNames } from '../sectionsNames';

class StudentSideMenu extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
        };
    }

    logOut = () => {
        const {
            logOut
        } = this.props;

        logOut();
    }

    changeSectionName = (name) => {
        const {
            changeSection
        } = this.props;

        changeSection(name);
    }

    render() {

        return (
            <Fragment className="block" >
                <Menu
                    defaultSelectedKeys={['1']}
                    defaultOpenKeys={['sub1']}
                    mode="inline"
                    theme="dark"
                >
                    <Menu.Item key="1" icon={<DesktopOutlined />} onClick={() => this.changeSectionName(sectionsNames.COURSES)}>
                        Courses
                    </Menu.Item>
                    <Menu.Item key="2" icon={<AppstoreOutlined />} onClick={() => this.changeSectionName(sectionsNames.SUBSCRIPTIONS)}>
                        Subscriptions
                    </Menu.Item>
                    <Menu.Item key="4" icon={<ExportOutlined />} onClick={() => this.logOut()}>
                        Log out
                    </Menu.Item>
                </Menu>
            </Fragment>
        );
    }
}

export default StudentSideMenu;