import React from 'react';
import coursesService from '../../../services/courses';
import { alertTypes } from '../../alert/types';
import Course from './course/index';

class AllCourses extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            courses: [],

            currentPage: this.props.currentPage,
            elementsOnPage: this.props.elementsOnPage,
            sortDirection: this.props.sortDirection,
            sortBy: this.props.sortBy,

            isSortingChanged: this.props.isSortingChanged,
            isElementsChanged: this.props.isElementsChanged
        };
    }

    componentDidMount() {

        document.title = "Courses";

        var model = this.formRequestModel();

        this.getCourses(model);
    }

    componentDidUpdate() {

        if (this.state.isElementsChanged || this.state.isSortingChanged) {

            var model = this.formRequestModel();
            this.getCourses(model);
        }
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        if (nextProps.isSortingChanged || nextProps.isElementsChanged) {

            return {
                elementsOnPage: nextProps.elementsOnPage,
                sortDirection: nextProps.sortDirection,
                sortBy: nextProps.sortBy,
                currentPage: nextProps.currentPage,

                isSortingChanged: nextProps.isSortingChanged,
                isElementsChanged: nextProps.isElementsChanged
            }
        }
    }

    getCourses = (model) => {

        const {
            startLoading,
            resetIsSortChangedStatus,
            finishLoading,
            setTotalCount,
            clearTotalCount
        } = this.props;

        if (this.state.isSortingChanged) {
            resetIsSortChangedStatus();

            this.setState({
                isSortingChanged: false
            })
        }

        startLoading();

        coursesService.getAllCourses(model)
            .then((response) => {

                this.setState({
                    courses: response.data.courses
                });

                setTotalCount(response.data.totalSize);
            },
                err => {
                    clearTotalCount();
                    this.setWarningAlert();
                })
            .catch(err => {
                clearTotalCount();
                console.log("Frontend error", err);
            })
            .finally(() => {
                finishLoading();
            });
    }

    setWarningAlert = () => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: "Something went wrong, try again !"
        }

        setAlert(model);
    }

    formRequestModel = () => {

        return {
            pageNumber: this.state.currentPage,
            elemOnPage: this.state.elementsOnPage,
            sortDirection: this.state.sortDirection,
            sortBy: this.state.sortBy
        }
    }

    render() {

        return (
            <>
                <div>
                    {this.state.courses.map((info, index) =>
                        <Course info={info} />
                    )}
                </div>
            </>
        );
    }
}

export default AllCourses;