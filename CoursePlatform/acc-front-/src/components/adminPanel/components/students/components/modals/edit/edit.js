import React from 'react';
import { Modal, Form, Input, Button } from 'antd';
import coursesService from '../../../../../../studentPanel/service';

const { TextArea } = Input;

class Edit extends React.Component {

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

    closeModal = (newData, oldEmail) => {

        const {
            clearModal
        } = this.props;

        clearModal({ newInfo: newData, email: oldEmail });
    };

    editStudent = (values) => {

        const {
            editStudentStarted,
            editStudentSuccess,
            editStudentFailed
        } = this.props;

        editStudentStarted();
        
        if (values.name == undefined &&
            values.surname == undefined &&
            values.age == undefined &&
            values.email == undefined) {

            editStudentSuccess();
            this.closeModal(this.props.data, this.props.data.email);

            return;
        }
        if (values.name == undefined) {
            values.name = this.props.data.name;
        }
        if (values.surname == undefined) {
            values.surname = this.props.data.surname;
        }
        if (values.email == undefined) {
            values.email = this.props.data.email;
        }
        if (values.age == undefined) {
            values.age = this.props.data.age;
        }

        var model = {
            user: {
                name: values.name,
                surname: values.surname,
                age: values.age,
                email: values.email
            },
            currentUserEmail: this.props.data.email
        }

        this.closeModal(model.user, this.props.data.email);

        coursesService.editStudent(model)
            .then(() => {
                editStudentSuccess();
            },
                err => {
                    editStudentFailed();
                })
            .catch(err => {
                console.log("Error");
            })
    };

    render() {

        return (
            <Modal title="Edit the student"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}
                destroyOnClose={"true"}>

                <Form {... this.state.layout} name="basic"
                    onFinish={(values) => this.editStudent(values)}
                    style={{ "textAlign": 'right' }}
                >

                    <Form.Item label="Enter new name :" name="name">
                        <TextArea className="name" allowClear
                            showCount autoSize maxLength={15}
                            defaultValue={this.props.data.name}
                            value={this.props.data.name}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new surname :" name="surname">
                        <TextArea className="surname" allowClear
                            showCount autoSize maxLength={25}
                            defaultValue={this.props.data.surname}
                            value={this.props.data.surname}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new email :" name="email">
                        <TextArea className="email"
                            autoSize allowClear maxLength={25}
                            defaultValue={this.props.data.email}
                            value={this.props.data.email}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new age :" name="age">
                        <TextArea className="age"
                            autoSize allowClear
                            defaultValue={this.props.data.age}
                            value={this.props.data.age}
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

export default Edit;