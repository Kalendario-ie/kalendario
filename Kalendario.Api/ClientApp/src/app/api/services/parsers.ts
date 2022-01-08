import {
    Colour,
    ServiceAdminResourceModel,
    ServiceCategoryAdminResourceModel,
    UpsertServiceCategoryCommand,
    UpsertServiceCommand
} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import {timeFromString, timeToISOString, Zero} from 'src/app/api/common/models';
import {Service, ServiceCategory} from 'src/app/api/services/models';
import {UpsertServiceCategoryRequest, UpsertServiceRequest} from 'src/app/api/services/requests';


export function serviceParser(data?: any): Service {
    return {
        ...data,
        permissionModel: PermissionModel.service,
        duration: timeFromString(data.duration)
    }
}

export function serviceCategoryParser(data: any): ServiceCategory {
    return {
        ...data
    }
}

export function createUpsertServiceCommand(service: ServiceAdminResourceModel | null | undefined): UpsertServiceCommand {
    return service ? {
        name: service.name,
        description: service.description,
        price: service.price,
        duration: service.duration,
        serviceCategoryId: timeToISOString(service.duration),
    } : {
        name: '',
        description: '',
        price: 0,
        duration: Zero(),
        serviceCategoryId: '',
    }
}

export function createUpsertServiceCategoryRequest(category: ServiceCategoryAdminResourceModel | null | undefined): UpsertServiceCategoryCommand {
    return category ? {
        name: category.name,
        colour: category.colour || {code: '#FFFFFF'}
    } : {
        name: '',
        colour: {code: '#FFFFFF'},
    }
}
