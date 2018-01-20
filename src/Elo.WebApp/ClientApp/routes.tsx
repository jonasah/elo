import * as React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { Layout } from './components/Layout';
import { HomePage } from './components/Home/HomePage';
import { RatingsPage } from './components/Ratings/RatingsPage';
import { PlayerStatsPage } from './components/PlayerStats/PlayerStatsPage';
import { GamesPage } from './components/Games/GamesPage';

export const routes = <Layout>
    <Route path='/' exact component={HomePage} />
    <Route path='/ratings' component={RatingsPage} />
    <Route path='/playerstats' component={PlayerStatsPage} />
    <Route path='/games' component={GamesPage} />
</Layout>;
