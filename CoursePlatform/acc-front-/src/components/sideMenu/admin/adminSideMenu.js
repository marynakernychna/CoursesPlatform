import React, { Fragment } from 'react';
import { Menu } from 'antd';
import {
    DatabaseFilled,
    ContactsFilled,
    ExportOutlined
} from '@ant-design/icons';
import { sectionsNames } from '../sectionsNames';
import { Link } from 'react-router-dom';
import { logOut } from '../../../reduxActions/general';
import { useDispatch } from 'react-redux';

const { SubMenu } = Menu;

function AdminSideMenu() {

    const dispatch = useDispatch();

    const logOut_ = () => {

        dispatch(logOut());
    }

    return (
        <div className="block" >
            <Menu
                defaultSelectedKeys={['/admin/courses']}
                defaultOpenKeys={['sub1']}
                mode="inline"
                theme="dark"
            >
                <SubMenu key="sub1" icon={<DatabaseFilled />} title="Courses">
                    <Menu.Item key="/admin/courses">
                        <Link to="/admin/courses">
                            View / Edit / Remove
                        </Link>
                    </Menu.Item>
                    <Menu.Item key="/admin/add-course">
                        <Link to="/admin/add-course">
                            Add
                        </Link>
                    </Menu.Item>
                </SubMenu>
                <SubMenu key="sub2" icon={<ContactsFilled />} title="Students">
                    <Menu.Item key="/admin/students">
                        <Link to="/admin/students">
                            View / Edit / Remove
                        </Link>
                    </Menu.Item>
                </SubMenu>
                <Menu.Item key="1" icon={<ExportOutlined />} onClick={logOut_}>
                    Log out
                </Menu.Item>
            </Menu>
        </div>
    );
}

export default AdminSideMenu;