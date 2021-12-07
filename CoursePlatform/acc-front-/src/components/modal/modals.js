import React from 'react';
import { modalsTypes } from './modalsTypes';
import Enroll from "./enroll/index";
import Unsubscribe from "./unsubscribe/index";
import Edit from './edit/index';

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