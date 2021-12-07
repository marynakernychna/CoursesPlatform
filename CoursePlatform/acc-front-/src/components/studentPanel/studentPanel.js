import React from 'react';
import { Layout } from 'antd';
import styles from './styles.module.css';
import CoursesPage from '../courses/index';
import { sectionsNames } from '../sideMenu/sectionsNames';
import { sideMenuTypes } from '../sideMenu/types';
import SideMenu from '../sideMenu/index';

const { Sider } = Layout

class StudentPanel extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      sectionName: this.props.sectionName
    };
  }

  static getDerivedStateFromProps = (nextProps, prevState) => {

    return {
      sectionName: nextProps.sectionName
    }
  }

  showSectionContent = () => {

    switch (this.state.sectionName) {
      case sectionsNames.COURSES: {
        return <CoursesPage type={sectionsNames.COURSES} />
      }
      case sectionsNames.SUBSCRIPTIONS: {
        return <CoursesPage type={sectionsNames.SUBSCRIPTIONS} />
      }
    }
  }

  render() {

    return (
      <Layout className={styles.pageBlock}>

        <Sider className={styles.sideMenu}>
          <SideMenu type={sideMenuTypes.STUDENT} />
        </Sider>

        {this.showSectionContent()}

      </Layout>
    );
  }
}

export default StudentPanel;