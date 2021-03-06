import {Profile} from 'oidc-client';
import React from 'react';
import {FormattedMessage} from 'react-intl';
import {Link} from 'react-router-dom';
import {
    Collapse,
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Nav,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
    UncontrolledDropdown,
} from 'reactstrap';
import {CompanyDetailsResourceModel} from 'src/app/api/api';
import {hasPermission, PermissionModel, PermissionType} from 'src/app/api/auth';
import {RequestModel} from 'src/app/api/requests';
import {ADMIN_ROUTES} from 'src/app/modules/admin/urls';
import {companiesUrls} from 'src/app/modules/companies/paths';
import {EMPLOYEE_ROUTES} from 'src/app/modules/employee/urls';
import {USER_ROUTES} from 'src/app/modules/users/urls';
import AvatarImg from 'src/app/shared/components/primitives/avatar-img';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';
import {ApplicationPaths} from 'src/components/api-authorization/ApiAuthorizationConstants';

interface AppNavbarProps {
    company: CompanyDetailsResourceModel | null;
    cart: RequestModel | null;
    cartIsEmpty: boolean;
    user: Profile | null;
    menuOpen: boolean;
    toggleMenu: () => void;
    showSidenavToggle: boolean;
    toggleSidenav: () => void;
    logoutPath: { pathname: string, state: { local: boolean } };
}

const AppNavbar: React.FunctionComponent<AppNavbarProps> = (
    {
        company,
        cart,
        cartIsEmpty,
        user,
        menuOpen,
        toggleMenu,
        showSidenavToggle,
        toggleSidenav,
        logoutPath,
    }) => {

    return (
        <header>
            <Navbar className="k-shadow-0" light expand="md">
                {showSidenavToggle &&
                <KIconButton onClick={toggleSidenav} icon="bars"/>
                }
                <NavbarBrand tag={Link} to="/" className="nav-logo">Kalendario</NavbarBrand>
                <NavbarToggler onClick={toggleMenu}/>
                <Collapse className="justify-content-end" isOpen={menuOpen} navbar>
                    <Nav navbar>
                        {company &&
                        <NavItem>
                            <NavLink tag={Link} to={companiesUrls(company).index}>
                                <AvatarImg src={company.avatar || ''} size={2} id="TooltipExample"/>
                            </NavLink>
                        </NavItem>
                        }
                        {cart &&
                        <NavItem>
                            <NavLink tag={Link} to={companiesUrls(company!).cart} disabled={cartIsEmpty}>
                                <i className="fa fa-shopping-cart"/>
                                <span className="badge">{cart.itemsCount}</span>
                            </NavLink>
                        </NavItem>
                        }
                        {!user &&
                        <>
                            <NavItem>
                                <NavLink tag={Link} to={ApplicationPaths.Login}><FormattedMessage
                                    id={'AUTH.LOGIN'}/></NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} to={ApplicationPaths.Register}><FormattedMessage
                                    id={'AUTH.REGISTER'}/></NavLink>
                            </NavItem>
                        </>
                        }
                        {user &&
                        <UncontrolledDropdown nav inNavbar>
                            <DropdownToggle nav caret>
                                Menu
                            </DropdownToggle>
                            <DropdownMenu right>
                                <DropdownItem>
                                    <NavLink tag={Link} to={USER_ROUTES.BOOKING()}>
                                        <FormattedMessage id={'USER.BOOKINGS'}/>
                                    </NavLink>
                                </DropdownItem>
                                <DropdownItem divider/>
                                {user.AccountId && hasPermission(user, PermissionType.view, PermissionModel.account) &&
                                <DropdownItem>
                                    <NavLink tag={Link} to={ADMIN_ROUTES.ROOT}>
                                        <FormattedMessage id={'NAVBAR.ADMIN'}/>
                                    </NavLink>
                                </DropdownItem>
                                }
                                {user.EmployeeId &&
                                <DropdownItem>
                                    <NavLink tag={Link} to={EMPLOYEE_ROUTES.ROOT}>
                                        <FormattedMessage id={'NAVBAR.EMPLOYEE'}/>
                                    </NavLink>
                                </DropdownItem>
                                }
                                <DropdownItem divider/>
                                <DropdownItem>
                                    <NavLink tag={Link} to={logoutPath}>
                                        <FormattedMessage id={'AUTH.LOGOUT'}/>
                                    </NavLink>
                                </DropdownItem>
                            </DropdownMenu>
                        </UncontrolledDropdown>
                        }
                    </Nav>
                </Collapse>
            </Navbar>
        </header>
    )
}

export default AppNavbar;
