import * as React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { Layout } from './components/Layout';
import { RatingsPage } from './components/RatingsPage';

export const routes = <Layout>
    <Route exact path='/' component={RatingsPage} />
    <Route path='/ratings' component={RatingsPage} />
</Layout>;
