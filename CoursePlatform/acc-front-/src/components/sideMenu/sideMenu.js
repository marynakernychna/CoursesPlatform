import React from 'react';
import { sideMenuTypes } from './types';
import StudentSideMenu from './students/index';
import AdminSideMenu from './admin/index';

class SideMenu extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
        };
    }

    showMenu = () => {
        
        switch (this.props.type) {
            case sideMenuTypes.ADMIN: {
                return <AdminSideMenu />
            }
            case sideMenuTypes.STUDENT: {
                return <StudentSideMenu />
            }
        }
    }

    render() {

        return (
            <>
                {this.showMenu()}
            </>
        );
    }
}

export default SideMenu;