import React from 'react';
import { Menu } from 'antd';
import {
    DesktopOutlined,
    AppstoreOutlined,
    ExportOutlined,
    UserOutlined
} from '@ant-design/icons';
import { Link, useHistory } from 'react-router-dom';
import { logOut } from '../../../reduxActions/general';
import { useDispatch } from 'react-redux';

function StudentSideMenu() {

    const dispatch = useDispatch();

    const logOut_ = () => {
        
        dispatch(logOut());
    }

    // const history = useHistory();

    return (
        <div className="block" >
            <Menu
                defaultSelectedKeys={["/student/courses"]}
                mode="inline"
                theme="dark"
            >
                <Menu.Item key="/student/courses" icon={<DesktopOutlined />} >
                    <Link to="/student/courses">
                        Courses
                    </Link>
                </Menu.Item>
                <Menu.Item key="/student/subscriptions" icon={<AppstoreOutlined />} >
                    <Link to="/student/subscriptions">
                        Subscriptions
                    </Link>
                </Menu.Item>
                <Menu.Item key="/student/profile" icon={<UserOutlined />} >
                    <Link to="/student/profile">
                        Profile
                    </Link>
                </Menu.Item>
                <Menu.Item key="1" icon={<ExportOutlined />}
                onClick={logOut_}>
                    Log out
                </Menu.Item>
            </Menu>
        </div>
    );
}

export default StudentSideMenu;