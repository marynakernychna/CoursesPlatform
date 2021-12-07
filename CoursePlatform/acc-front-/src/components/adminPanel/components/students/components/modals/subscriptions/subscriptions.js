import React from 'react';
import { Modal, Form, Table } from 'antd';

class Subscriptions extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            modalTitle: this.props.data.name +
                " " + this.props.data.surname +
                " subscriptions",
            layout: {
                labelCol: {
                    span: 8,
                },
                wrapperCol: {
                    span: 16,
                },
            },
            columns: [
                {
                    title: 'Title',
                    dataIndex: 'title',
                },
                {
                    title: 'Description',
                    dataIndex: 'description',
                }
            ]
        };
    }

    closeModal = () => {

        const {
            clearModal
        } = this.props;

        clearModal();
    };

    render() {

        return (
            <Modal title={this.state.modalTitle}
                visible={true}
                onCancel={this.closeModal}
                afterClose={this.closeModal}
                onOk={this.closeModal}
                cancelButtonProps={{ style: { display: 'none' } }}>
                <Form {... this.state.layout} name="basic"
                    style={{ "textAlign": 'right' }}>

                    <Table columns={this.state.columns} dataSource={this.props.data.subscriptions} size="small" />

                </Form>
            </Modal>
        );
    }
}

export default Subscriptions;