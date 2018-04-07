import {applyMiddleware, createStore} from 'redux';
import reducers from './reducers/index.js';
import thunk from 'redux-thunk';
import logger from 'redux-logger';

const middlware = applyMiddleware(thunk);

const store = createStore(reducers, middlware);

export default store;