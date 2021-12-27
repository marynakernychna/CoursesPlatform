import React from 'react';
import { Button, Dropdown, Space, Menu } from 'antd';
import { elementsOnPage } from '../../constants/elementsOnPageCount';

class ElementsOnPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            elementsOnPage: this.props.elementsOnPage,
            totalElementsCount: this.props.totalElementsCount
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            elementsOnPage: nextProps.elementsOnPage,
            totalElementsCount: nextProps.totalElementsCount
        }
    }

    changeElementsOnPageCount = (onPage) => {

        var total = this.state.totalElementsCount;

        if (this.state.elementsOnPage != onPage &&
            total > 1 &&
            this.state.elementsOnPage < total) {

            const {
                changeElementsOnPageCount
            } = this.props;

            changeElementsOnPageCount(onPage);
        }
    }

    render() {

        const elementsOnPageMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(elementsOnPage[5])}>
                        5
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(elementsOnPage[10])}>
                        10
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(elementsOnPage[20])}>
                        20
                    </a>
                </Menu.Item>
            </Menu>
        );

        return (
            <Space direction="vertical">
                <Space wrap>
                    <Dropdown overlay={elementsOnPageMenu} placement="bottomCenter">
                        <Button>Elements on page</Button>
                    </Dropdown>
                </Space>
            </Space>
        );
    }
}

export default ElementsOnPage;