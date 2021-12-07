import React, { Fragment } from 'react';
import { Menu } from 'antd';
import {
    DatabaseFilled,
    ContactsFilled,
    ExportOutlined
} from '@ant-design/icons';
import { sectionsNames } from '../sectionsNames';

const { SubMenu } = Menu;

class AdminSideMenu extends React.Component {

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

    changeSection = (name) => {
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
                    <SubMenu key="sub1" icon={<DatabaseFilled />} title="Courses">
                        <Menu.Item key="1" onClick={() => this.changeSection(sectionsNames.COURSES)}>View / Edit / Remove</Menu.Item>
                        <Menu.Item key="2" onClick={() => this.changeSection(sectionsNames.ADDCOURSE)}>Add</Menu.Item>
                    </SubMenu>
                    <SubMenu key="sub2" icon={<ContactsFilled />} title="Students">
                        <Menu.Item key="3" onClick={() => this.changeSection(sectionsNames.STUDENTS)}>View / Edit / Remove</Menu.Item>
                    </SubMenu>
                    <Menu.Item key="5" icon={<ExportOutlined />} onClick={() => this.logOut()}>
                        Log out
                    </Menu.Item>
                </Menu>
            </Fragment>
        );
    }
}

export default AdminSideMenu;