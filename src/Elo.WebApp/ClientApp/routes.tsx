import * as React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { Layout } from './components/Layout';
import { RatingsPage } from './components/Ratings/RatingsPage';
import { PlayerStatsPage } from './components/PlayerStats/PlayerStatsPage';
import { GamesPage } from './components/Games/GamesPage';

export const routes = <Layout>
    <Route exact path='/' component={RatingsPage} />
    <Route path='/playerstats/:player' component={PlayerStatsPage} />
    <Route path='/games' component={GamesPage} />
</Layout>;
