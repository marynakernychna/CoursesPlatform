import React from 'react';
import { Modal, Form } from 'antd';
import coursesService from '../../../../../../studentPanel/service';

class Remove extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            layout: {
                labelCol: {
                    span: 8,
                },
                wrapperCol: {
                    span: 16,
                },
            },
            tailLayout: {
                wrapperCol: {
                    offset: 8,
                    span: 16,
                },
            }
        };
    }

    closeModal = () => {
        
        const {
            clearModal
        } = this.props;

        clearModal();
    };

    removeStudent = () => {

        const {
            removeStudentStarted,
            removeStudentSuccess,
            removeStudentFailed
        } = this.props;

        removeStudentStarted();

        var model = {
            Value: this.props.data.email
        }

        this.closeModal();
        
        coursesService.removeStudent(model)
            .then(() => {

                removeStudentSuccess();
            },
                err => {
                    removeStudentFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    }
    
    render() {

        return (
            <Modal title="Confirm removing"
                    visible={true}
                    onCancel={() => this.closeModal()}
                    afterClose={() => this.closeModal()}
                    onOk={() => this.removeStudent()}>

                    <Form {... this.state.layout} name="basic"
                        style={{ "textAlign": 'center' }}>

                        <h4>Remove "{this.props.data.name} {this.props.data.surname}" student?</h4>

                    </Form>
                </Modal>
        );
    }
}

export default Remove;