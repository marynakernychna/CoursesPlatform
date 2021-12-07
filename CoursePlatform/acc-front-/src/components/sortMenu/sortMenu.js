import React from 'react';
import { Button, Dropdown, Space, Menu } from 'antd';
import { sortByTypes } from '../../actions/general/sortByTypes';
import { sortDirectionTypes } from '../../actions/general/sortDirectionTypes';
import { elementsOnPage } from '../../actions/general/elementsOnPageCount';

class SortMenu extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            elementsOnPage: this.props.elementsOnPage,
            sortDirection: this.props.sortDirection,
            sortBy: this.props.sortBy,
            totalElementsCount: this.props.totalElementsCount
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            elementsOnPage: nextProps.elementsOnPage,
            sortDirection: nextProps.sortDirection,
            sortBy: nextProps.sortBy,
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

    changeElementsDirectionSort = (direction) => {

        if (this.state.sortDirection != direction &&
            this.state.totalElementsCount > 1) {

            const {
                changeElementsSortDirection
            } = this.props;

            changeElementsSortDirection(direction);
        }
    }

    changeElementsSortBy = (sortBy) => {

        if (this.state.sortBy != sortBy &&
            this.state.totalElementsCount > 1) {

            const {
                changeElementsSortBy
            } = this.props;

            changeElementsSortBy(sortBy);
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

        const directionMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsDirectionSort(sortDirectionTypes.ASC)}>
                        by ascending
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsDirectionSort(sortDirectionTypes.DESC)}>
                        by descending
                    </a>
                </Menu.Item>
            </Menu>
        );

        const sortByMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsSortBy(sortByTypes.TITLE)}>
                        title
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsSortBy(sortByTypes.DATE)}>
                        creation date
                    </a>
                </Menu.Item>
            </Menu>
        );

        return (
            <>
                <Space direction="vertical">
                    <Space wrap>
                        <Dropdown overlay={elementsOnPageMenu} placement="bottomCenter">
                            <Button>Elements on page</Button>
                        </Dropdown>
                    </Space>
                </Space>

                <Space direction="vertical" style={{ marginLeft: '10px' }}>
                    <Space wrap>
                        <Dropdown overlay={directionMenu} placement="bottomCenter">
                            <Button>Sort direction</Button>
                        </Dropdown>
                    </Space>
                </Space>

                <Space direction="vertical" style={{ marginLeft: '10px' }}>
                    <Space wrap>
                        <Dropdown overlay={sortByMenu} placement="bottomCenter">
                            <Button>Sort by</Button>
                        </Dropdown>
                    </Space>
                </Space>
            </>
        );
    }
}

export default SortMenu;