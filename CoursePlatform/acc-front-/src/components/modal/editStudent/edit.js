import React from 'react';
import { Modal, Form, Button, Input } from 'antd';
import usersService from '../../../services/students';
import { alertTypes } from '../../alert/types';

const { TextArea } = Input;

class EditStudent extends React.Component {

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
            closeModal
        } = this.props;

        closeModal();
    };

    editStudent = (info) => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            updateTableData
        } = this.props;

        startLoading();

        if (info.name === undefined) {
            info.name = this.props.info.name;
        }
        if (info.surname === undefined) {
            info.surname = this.props.info.surname;
        }
        if (info.email === undefined) {
            info.email = this.props.info.email;
        }

        if (info.name == this.props.info.name &&
            info.surname == this.props.info.surname &&
            info.email == this.props.info.email) {

            this.closeModal();

            setAlert({
                type: alertTypes.INFO,
                message: "You have successfully edited the student !"
            });

            finishLoading();

            return;
        }

        info['id'] = this.props.info.id;

        var request = {
            user: {
                name: info.name,
                surname: info.surname,
                email: info.email,
                birthday: this.props.info.birthday
            },
            currentUserEmail: this.props.info.email
        }

        this.closeModal();

        usersService.editUser(request)
            .then(() => {

                updateTableData({ key: this.props.info.key, newData: info });
                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully edited the student !"
                });
            },
                err => {
                    
                    this.setWarningAlert(err.response);
                })
            .catch(err => {

                this.setWarningAlert();
            })
            .finally(() => {
                finishLoading();
            });
    };

    setWarningAlert = (err) => {

        const {
            setAlert
        } = this.props;
        
        var model = {
            type: alertTypes.WARNING,
            message: err.data.errors != undefined &&
                err.data.errors.Message != undefined ?
                err.data.errors.Message :
                "Something went wrong. Try again!"
        }

        setAlert(model);
    }

    render() {

        return (
            <Modal title="Edit the student"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}>

                <Form {... this.state.layout} name="basic"
                    onFinish={(values) => this.editStudent(values)}
                    style={{ "textAlign": 'right' }}
                >

                    <Form.Item label="Enter new name :" name="name">
                        <TextArea className="name" allowClear
                            showCount autoSize maxLength={20}
                            defaultValue={this.props.info.name}
                            value={this.props.info.name}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new surname :" name="surname">
                        <TextArea className="surname" allowClear
                            showCount autoSize maxLength={50}
                            defaultValue={this.props.info.surname}
                            value={this.props.info.surname}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new email :" name="email">
                        <TextArea className="email"
                            showCount autoSize maxLength={50}
                            defaultValue={this.props.info.email}
                            value={this.props.info.email}
                        />
                    </Form.Item>

                    <Form.Item {... this.state.tailLayout}
                        style={{ "marginTop": '10px' }}>
                        <Button type="primary" htmlType="submit">Submit</Button>
                    </Form.Item>

                </Form>

            </Modal>
        );
    }
}

export default EditStudent;