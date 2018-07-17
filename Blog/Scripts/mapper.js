"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DataMapperService = /** @class */ (function () {
    function DataMapperService() {
    }
    DataMapperService.prototype.mapObjectToPostCardModel = function (object) {
        var postCardModel = {
            postId: object["PostId"],
            title: object["Title"],
            createdDate: object["CreatedDate"],
            updatedDate: object["UpdatedDate"],
            shortDescription: object["ShortDescription"],
            thumbnailImageSrc: object["ThumbnailImageSrc"]
        };
        return postCardModel;
    };
    DataMapperService.prototype.mapObjectToPostCardPaginationModel = function (object) {
        var postCardPaginationModel = {
            postCardModels: object["Models"],
            hasNext: object["HasNext"],
            hasPrevious: object["HasPrevious"],
            pageNumber: object["PageNumber"],
            pages: object["Pages"]
        };
        return postCardPaginationModel;
    };
    return DataMapperService;
}());
exports.DataMapperService = DataMapperService;
//# sourceMappingURL=mapper.js.map