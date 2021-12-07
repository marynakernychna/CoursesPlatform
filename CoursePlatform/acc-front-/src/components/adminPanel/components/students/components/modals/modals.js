import React from 'react';
import * as modalTypes from './modalTypes';
import Edit from './edit/index'
import Remove from './remove/index';
import Subscriptions from './subscriptions/index';

class Modals extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            modal: {}
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {
        return {
            modal: nextProps.modal
        }
    }

    showModals = () => {

        switch (this.state.modal.type) {
            case modalTypes.EDIT: {
                return <Edit data={this.state.modal.data}/>
            }
            case modalTypes.REMOVE: {
                return <Remove data={this.state.modal.data}/>
            }
            case modalTypes.VIEWSUBSCRIPTIONS: {
                return <Subscriptions data={this.state.modal.data}/>
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