import React ,{useState} from 'react'
import User from './User'
import 'react-pro-sidebar/dist/css/styles.css';
import '../index.css'
import {ProSidebar,Menu,MenuItem,SidebarHeader,SidebarFooter,SidebarContent} from "react-pro-sidebar";
import {FaTachometerAlt,FaGem,FaHeart,FaClipboardList,FaUserCircle} from "react-icons/fa";
import {  FiLogOut,FiSettings} from "react-icons/fi";
import {FcMenu} from 'react-icons/fc';
import image from './../assets/sidebar-2.jpg';
import coin from'./../assets/coin.jpeg';
import {BrowserRouter as Router,Route,Link} from "react-router-dom";
import {Col,Container,Row} from 'react-bootstrap'
import Dashboard from './Dashboard';
import Employee from './Employee';
import Business from './Business';
import Settings from './Settings';
import Transaction from './Transaction';
import Test from './Test';
import Request from './Request';
const $ = require('jquery');
export default function SideNavPage() {
  const [Open, setOpen] = useState(false)
       function showSidebar(){
         
            $("#sidebar").removeClass("hideSidebar");
            $("#sidebar").addClass("ss");
        
        }
        function hideSidebar(){
          $("#sidebar").addClass("hideSidebar");
          $("#sidebar").removeClass("ss");
         
        }
 
    return (
     
      <Router>

     <Row>
      
     
      <Col md="2" lg="2">
       <div id="sidebar" className="hideSidebar ">
        
        <ProSidebar  className="ss" image={image} style={{ height: "100vh" }}  >
                <SidebarHeader  image={image}>
                <div 
                    style={{
                    padding: "24px",
                    textTransform: "uppercase",
                    fontWeight: "bold",
                    fontSize: 14,
                    letterSpacing: "1px",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                    whiteSpace: "nowrap",
                    }}
                >
                  <img src={coin}  style={{height:"70px"}}/>
                 
                </div>

          
                </SidebarHeader>
                <SidebarContent>
                <Menu iconShape="circle">
                    <MenuItem icon={<FaTachometerAlt />}> Dashboard<Link to="/"/></MenuItem>
                    <MenuItem icon={<FaHeart />}>Users List<Link to="/user"/> </MenuItem>
                    <MenuItem icon={<FaClipboardList />}>Business List<Link to="/business"/> </MenuItem>
                    <MenuItem icon={<FaUserCircle/>}>Employees List<Link to="/employee"/></MenuItem>
                   
                    <MenuItem icon={<FaGem/>}>Requests<Link to="/request"/></MenuItem>
                    
                    <MenuItem icon={<FaGem/>}> Operations <Link to ="/test"/></MenuItem>
                    <MenuItem icon={<FiSettings />}><Link to="/settings"/>Settings</MenuItem>
                </Menu>
                
                </SidebarContent>
                <SidebarFooter ><Menu iconShape="circle">
              <MenuItem icon={<FiLogOut />}>Logout</MenuItem>
            </Menu></SidebarFooter>
            </ProSidebar>
           
            </div>
            </Col>
            <Container>
            <Col className="m-3">
           
        <button id="button-id" onClick={() => {
         if (!Open)
          {
            showSidebar();
            setOpen(true);
          }
        else{
          hideSidebar();
          setOpen(false);
        }

       }}>{<FcMenu/>}</button>
      
           <Route exact path="/" component={Dashboard} />
           <Route path="/user" component={User} />
           <Route path="/employee" component={Employee}></Route>
           <Route path="/business" component={Business}></Route>
           <Route path="/settings" component={Settings}></Route>
           <Route path="/transaction" exact component={Transaction}></Route>
           <Route path="/transaction/:id" component={Transaction}></Route>
           <Route path="/request" component={Request}></Route>

           <Route path="/test" component={Test}></Route>
           
           
          
           
           </Col>
           </Container>
           </Row>
          
            </Router>
         
          
    )
}
