import React from 'react';
import { modalsTypes } from './modalsTypes';
import Enroll from "./enroll/index";
import Unsubscribe from "./unsubscribe/index";
import Edit from './editCourse/index';
import Remove from './removeCourse/index';
import EditStudent from './editStudent/index';
import RemoveStudent from './removeStudent/index';
import EditProfile from './editProfile/index';
import ChangePassword from './changePassword/index';

class Modals extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            modalInfo: this.props.modalInfo
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            modalInfo: nextProps.modalInfo
        }
    }

    showModals = () => {

        switch (this.state.modalInfo.type) {
            case modalsTypes.ENROLL: {
                return <Enroll info={this.state.modalInfo.info} />
            }
            case modalsTypes.UNSUBSCRIBE: {
                return <Unsubscribe info={this.state.modalInfo.info} />
            }
            case modalsTypes.EDIT_COURSE: {
                return <Edit info={this.state.modalInfo.info} />
            }
            case modalsTypes.REMOVE_COURSE: {
                return <Remove info={this.state.modalInfo.info} />
            }
            case modalsTypes.EDIT_STUDENT: {
                return <EditStudent info={this.state.modalInfo.info} />
            }
            case modalsTypes.REMOVE_STUDENT: {
                return <RemoveStudent info={this.state.modalInfo.info} />
            }
            case modalsTypes.EDIT_PROFILE: {
                return <EditProfile info={this.state.modalInfo.info} />
            }
            case modalsTypes.CHANGE_PASSWORD: {
                return <ChangePassword info={this.state.modalInfo.info} />
            }
        }
    }

    render() {

        return (
            <>
                {this.showModals()}
            </>
        );
    }
}

export default Modals;